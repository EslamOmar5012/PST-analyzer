using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using PstAnalyzer.Models;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace PstAnalyzer.Services
{
    /// <summary>
    /// Scans PST files via Outlook Interop and builds PstFileReport / MergeReport objects.
    /// This is a standalone service — it does NOT modify any PST files.
    /// </summary>
    public class PstAnalyzerService
    {
        // ── Progress callback: (percentDone 0-100, message) ──────────────────
        public event Action<int, string> Progress;

        private void Report(int pct, string msg) => Progress?.Invoke(pct, msg);

        // ── Hash helpers (same algorithm as PstMerger so dedup is comparable) ─

        private static string GetItemHash(object item)
        {
            try
            {
                string subject = "", sender = "", sentOn = "", size = "", bodyLen = "";
                dynamic dyn = item;

                try { subject = (string)dyn.Subject ?? ""; } catch { }
                try { sender  = (string)dyn.SenderName ?? ""; }
                catch { try { sender = (string)dyn.Organizer ?? ""; } catch { try { sender = (string)dyn.From ?? ""; } catch { } } }
                try { sentOn  = ((DateTime)dyn.SentOn).ToString("o"); }
                catch { try { sentOn = ((DateTime)dyn.Start).ToString("o"); } catch { try { sentOn = ((DateTime)dyn.CreationTime).ToString("o"); } catch { } } }
                try { size    = ((int)dyn.Size).ToString(); } catch { }
                try { bodyLen = ((string)dyn.Body ?? "").Length.ToString(); } catch { }

                string raw = string.Join("|", subject, sender, sentOn, size, bodyLen);
                using (var md5 = MD5.Create())
                {
                    byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(raw));
                    return BitConverter.ToString(bytes).Replace("-", "");
                }
            }
            catch { return null; }
        }

        private static string GetItemSubject(object item)
        {
            try { dynamic d = item; return (string)d.Subject ?? "(no subject)"; } catch { return "(no subject)"; }
        }
        private static string GetItemSender(object item)
        {
            try { dynamic d = item; return (string)d.SenderName ?? ""; } catch { return ""; }
        }
        private static string GetItemSentOn(object item)
        {
            try { dynamic d = item; return ((DateTime)d.SentOn).ToString("yyyy-MM-dd HH:mm"); }
            catch { try { dynamic d = item; return ((DateTime)d.Start).ToString("yyyy-MM-dd HH:mm"); } catch { return ""; } }
        }

        // ── Outlook item type helpers ─────────────────────────────────────────

        private enum ItemKind { Email, Calendar, Contact, Task, Note, Other }

        private static ItemKind ClassifyItem(object item)
        {
            try
            {
                dynamic d = item;
                int cls = (int)d.Class;
                switch (cls)
                {
                    case 43:  return ItemKind.Email;
                    case 26:  return ItemKind.Calendar;
                    case 40:  return ItemKind.Contact;
                    case 48:  return ItemKind.Task;
                    case 44:  return ItemKind.Note;
                    default:  return ItemKind.Other;
                }
            }
            catch { return ItemKind.Other; }
        }

        // ═════════════════════════════════════════════════════════════════════
        // PUBLIC API
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Scans all PST files in sourceFiles and (optionally) the already-merged PST,
        /// then returns a complete MergeReport.
        /// </summary>
        public MergeReport Analyze(
            string[] sourceFiles,
            string mergedPstPath,        // pass null / "" to skip post-merge analysis
            CancellationToken ct)
        {
            var report = new MergeReport();
            report.ReportGeneratedAt = DateTime.Now;

            Outlook.Application outlookApp = null;
            Outlook.NameSpace ns = null;

            try
            {
                Report(0, "Starting Outlook...");
                outlookApp = new Outlook.Application();
                ns = outlookApp.GetNamespace("MAPI");

                // ── 1. Scan each source PST ───────────────────────────────────
                // Global hash map: hash → list of (file, folderPath) for cross-file dup detection
                var globalHashes = new Dictionary<string, List<(string file, string folder)>>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < sourceFiles.Length; i++)
                {
                    if (ct.IsCancellationRequested) break;

                    string file = sourceFiles[i];
                    int pct = (int)((i / (double)sourceFiles.Length) * 70);
                    Report(pct, string.Format("Scanning ({0}/{1}): {2}", i + 1, sourceFiles.Length, Path.GetFileName(file)));

                    var fileReport = ScanPst(ns, file, globalHashes, ct);
                    report.SourceReports.Add(fileReport);
                }

                // ── 2. Build cross-file duplicate groups ──────────────────────
                Report(75, "Analysing cross-file duplicates...");
                foreach (var kvp in globalHashes)
                {
                    if (kvp.Value.Count > 1)
                    {
                        var grp = new DuplicateGroup
                        {
                            Hash  = kvp.Key,
                            Count = kvp.Value.Count
                        };
                        foreach (var (file, folder) in kvp.Value)
                            grp.Occurrences.Add(new DuplicateOccurrence { SourceFile = file, FolderPath = folder });
                        report.CrossFileDuplicates.Add(grp);
                    }
                }

                // Stamp per-file cross-file dup count
                foreach (var fr in report.SourceReports)
                {
                    fr.DuplicatesFoundAcrossFiles = 0;
                    foreach (var grp in report.CrossFileDuplicates)
                    {
                        foreach (var occ in grp.Occurrences)
                        {
                            if (string.Equals(occ.SourceFile, fr.FileName, StringComparison.OrdinalIgnoreCase))
                                fr.DuplicatesFoundAcrossFiles++;
                        }
                    }
                    fr.DuplicateGroups.AddRange(report.CrossFileDuplicates);
                }

                // ── 3. Scan merged PST (if it exists) ─────────────────────────
                if (!string.IsNullOrWhiteSpace(mergedPstPath) && File.Exists(mergedPstPath))
                {
                    Report(80, "Scanning merged PST: " + Path.GetFileName(mergedPstPath));
                    var empty = new Dictionary<string, List<(string, string)>>();
                    report.MergedReport = ScanPst(ns, mergedPstPath, empty, ct);
                }

                Report(100, "Analysis complete.");
            }
            catch (OperationCanceledException)
            {
                Report(0, "Analysis cancelled.");
            }
            finally
            {
                if (ns != null) try { Marshal.ReleaseComObject(ns); } catch { }
                if (outlookApp != null) try { Marshal.ReleaseComObject(outlookApp); } catch { }
            }

            return report;
        }

        // ═════════════════════════════════════════════════════════════════════
        // PRIVATE: scan one PST file
        // ═════════════════════════════════════════════════════════════════════

        private PstFileReport ScanPst(
            Outlook.NameSpace ns,
            string filePath,
            Dictionary<string, List<(string file, string folder)>> globalHashes,
            CancellationToken ct)
        {
            var report = new PstFileReport
            {
                FilePath         = filePath,
                FileName         = Path.GetFileName(filePath),
                ScanTime         = DateTime.Now,
                FileSizeBytes    = new FileInfo(filePath).Length,
                FileModifiedDate = File.GetLastWriteTime(filePath)
            };

            Outlook.Folder root = null;
            try
            {
                ns.AddStore(filePath);
                root = GetRootFolder(ns, filePath);
                if (root == null)
                {
                    report.ScanError = "Could not open PST root folder.";
                    return report;
                }

                // Local hash set to find within-file duplicates
                var localHashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                report.RootFolder = ScanFolder(root, report.FileName, "", localHashes, globalHashes, report, ct);

                ns.RemoveStore(root);
                Marshal.ReleaseComObject(root);
                root = null;
            }
            catch (Exception ex)
            {
                report.ScanError = ex.Message;
                if (root != null) try { Marshal.ReleaseComObject(root); } catch { }
            }

            return report;
        }

        // ── Recursive folder scanner ──────────────────────────────────────────

        private FolderStats ScanFolder(
            Outlook.Folder folder,
            string fileName,
            string parentPath,
            HashSet<string> localHashes,
            Dictionary<string, List<(string file, string folder)>> globalHashes,
            PstFileReport report,
            CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return null;

            string currentPath = string.IsNullOrEmpty(parentPath)
                ? folder.Name
                : parentPath + "\\" + folder.Name;

            var stats = new FolderStats
            {
                FolderName = folder.Name,
                FolderPath = currentPath
            };

            // ── Scan items ────────────────────────────────────────────────────
            Outlook.Items items = null;
            try
            {
                items = folder.Items;
                int count = items.Count;
                stats.TotalItems = count;
                report.TotalItems  += count;
                report.TotalFolders++;

                for (int i = 1; i <= count; i++)
                {
                    if (ct.IsCancellationRequested) break;
                    object item = null;
                    try
                    {
                        item = items[i];

                        // Classify
                        var kind = ClassifyItem(item);
                        switch (kind)
                        {
                            case ItemKind.Email:    stats.EmailItems++;    report.TotalEmails++;    break;
                            case ItemKind.Calendar: stats.CalendarItems++; report.TotalCalendar++;  break;
                            case ItemKind.Contact:  stats.ContactItems++;  report.TotalContacts++;  break;
                            case ItemKind.Task:     stats.TaskItems++;     report.TotalTasks++;     break;
                            case ItemKind.Note:     stats.NoteItems++;     report.TotalNotes++;     break;
                            default:                stats.OtherItems++;    report.TotalOther++;     break;
                        }

                        // Size
                        try { dynamic d = item; stats.TotalSizeBytes += (int)d.Size; } catch { }

                        // Hash for dedup
                        string hash = GetItemHash(item);
                        if (!string.IsNullOrEmpty(hash))
                        {
                            // Within-file duplicate
                            if (localHashes.Contains(hash))
                                report.DuplicatesWithinFile++;
                            else
                                localHashes.Add(hash);

                            // Cross-file duplicate tracking
                            if (!globalHashes.ContainsKey(hash))
                                globalHashes[hash] = new List<(string, string)>();
                            globalHashes[hash].Add((fileName, currentPath));
                        }
                    }
                    catch { /* skip bad item */ }
                    finally
                    {
                        if (item != null) try { Marshal.ReleaseComObject(item); } catch { }
                    }
                }
            }
            catch { }
            finally
            {
                if (items != null) try { Marshal.ReleaseComObject(items); } catch { }
            }

            // ── Recurse into sub-folders ──────────────────────────────────────
            Outlook.Folders subFolders = null;
            try
            {
                subFolders = folder.Folders;
                foreach (Outlook.Folder sub in subFolders)
                {
                    if (ct.IsCancellationRequested) break;
                    try
                    {
                        var childStats = ScanFolder(sub, fileName, currentPath, localHashes, globalHashes, report, ct);
                        if (childStats != null) stats.SubFolders.Add(childStats);
                    }
                    catch { }
                    finally { try { Marshal.ReleaseComObject(sub); } catch { } }
                }
            }
            catch { }
            finally
            {
                if (subFolders != null) try { Marshal.ReleaseComObject(subFolders); } catch { }
            }

            return stats;
        }

        // ── GetRootFolder (same logic as PstMerger) ───────────────────────────

        private Outlook.Folder GetRootFolder(Outlook.NameSpace ns, string filePath)
        {
            Outlook.Store targetStore = null;
            foreach (Outlook.Store store in ns.Stores)
            {
                try
                {
                    if (string.Equals(store.FilePath, filePath, StringComparison.OrdinalIgnoreCase))
                    { targetStore = store; break; }
                }
                catch { }
            }

            if (targetStore != null)
            {
                try
                {
                    const string PR = "http://schemas.microsoft.com/mapi/proptag/0x35E00102";
                    object prop = targetStore.PropertyAccessor.GetProperty(PR);
                    string eid = prop is byte[] b
                        ? BitConverter.ToString(b).Replace("-", "")
                        : prop as string;

                    if (!string.IsNullOrEmpty(eid))
                    {
                        var f = ns.GetFolderFromID(eid, targetStore.StoreID) as Outlook.Folder;
                        if (f != null) return f;
                    }
                }
                catch { }
            }

            // Fallback
            foreach (Outlook.Folder folder in ns.Folders)
            {
                try
                {
                    if (folder.Store != null &&
                        string.Equals(folder.Store.FilePath, filePath, StringComparison.OrdinalIgnoreCase))
                        return folder;
                }
                catch { }
                try { Marshal.ReleaseComObject(folder); } catch { }
            }
            return null;
        }
    }
}
