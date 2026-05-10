using System;
using System.Collections.Generic;

namespace PstAnalyzer.Models
{
    /// <summary>
    /// Full analysis report for one PST file (before merge).
    /// </summary>
    public class PstFileReport
    {
        // ── Identity ──────────────────────────────────────────────────────────
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public DateTime FileModifiedDate { get; set; }

        // ── Totals ────────────────────────────────────────────────────────────
        public int TotalItems { get; set; }
        public int TotalEmails { get; set; }
        public int TotalCalendar { get; set; }
        public int TotalContacts { get; set; }
        public int TotalTasks { get; set; }
        public int TotalNotes { get; set; }
        public int TotalOther { get; set; }
        public int TotalFolders { get; set; }

        // ── Duplicate analysis ────────────────────────────────────────────────
        /// <summary>Items whose hash already appeared in a previous PST (cross-file dups).</summary>
        public int DuplicatesFoundAcrossFiles { get; set; }
        /// <summary>Items that are duplicated WITHIN this file.</summary>
        public int DuplicatesWithinFile { get; set; }
        /// <summary>Full list of duplicate groups involving this file.</summary>
        public List<DuplicateGroup> DuplicateGroups { get; set; } = new List<DuplicateGroup>();

        // ── Folder tree ───────────────────────────────────────────────────────
        public FolderStats RootFolder { get; set; }

        // ── Scan metadata ─────────────────────────────────────────────────────
        public DateTime ScanTime { get; set; }
        public string ScanError { get; set; }   // null if success
        public bool ScanSucceeded => string.IsNullOrEmpty(ScanError);

        // ── Helpers ───────────────────────────────────────────────────────────
        public string FileSizeDisplay =>
            FileSizeBytes >= 1_073_741_824
                ? string.Format("{0:F2} GB", FileSizeBytes / 1_073_741_824.0)
                : string.Format("{0:F2} MB", FileSizeBytes / 1_048_576.0);
    }
}
