using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PstAnalyzer.Models;

namespace PstAnalyzer.Services
{
    /// <summary>
    /// Generates a self-contained HTML report from a MergeReport.
    /// The report opens in any browser with no external dependencies.
    /// </summary>
    public static class HtmlReportGenerator
    {
        public static string Generate(MergeReport report)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<title>PST Merge Analysis Report</title>
<style>
  @import url('https://fonts.googleapis.com/css2?family=IBM+Plex+Mono:wght@400;600&family=IBM+Plex+Sans:wght@300;400;600;700&display=swap');

  :root {
    --bg: #0d1117;
    --surface: #161b22;
    --surface2: #1c2230;
    --border: #30363d;
    --accent: #58a6ff;
    --accent2: #3fb950;
    --warn: #d29922;
    --danger: #f85149;
    --text: #e6edf3;
    --muted: #8b949e;
    --mono: 'IBM Plex Mono', monospace;
    --sans: 'IBM Plex Sans', sans-serif;
  }

  *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

  body {
    background: var(--bg);
    color: var(--text);
    font-family: var(--sans);
    font-size: 14px;
    line-height: 1.6;
  }

  /* ── Header ── */
  .header {
    background: linear-gradient(135deg, #0d1117 0%, #1a2332 50%, #0d1117 100%);
    border-bottom: 1px solid var(--border);
    padding: 36px 48px 28px;
    position: relative;
    overflow: hidden;
  }
  .header::before {
    content: '';
    position: absolute;
    top: -60px; right: -60px;
    width: 300px; height: 300px;
    background: radial-gradient(circle, rgba(88,166,255,0.08) 0%, transparent 70%);
    pointer-events: none;
  }
  .header-tag {
    font-family: var(--mono);
    font-size: 11px;
    color: var(--accent);
    letter-spacing: 3px;
    text-transform: uppercase;
    margin-bottom: 8px;
  }
  .header h1 {
    font-size: 28px;
    font-weight: 700;
    color: var(--text);
    margin-bottom: 6px;
  }
  .header-meta {
    font-size: 12px;
    color: var(--muted);
    font-family: var(--mono);
  }

  /* ── Layout ── */
  .container { max-width: 1200px; margin: 0 auto; padding: 32px 48px; }

  /* ── Section titles ── */
  .section-title {
    font-size: 13px;
    font-weight: 600;
    letter-spacing: 2px;
    text-transform: uppercase;
    color: var(--accent);
    font-family: var(--mono);
    border-bottom: 1px solid var(--border);
    padding-bottom: 8px;
    margin: 40px 0 20px;
  }
  .section-title:first-child { margin-top: 0; }

  /* ── KPI cards ── */
  .kpi-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    gap: 12px;
    margin-bottom: 8px;
  }
  .kpi {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: 6px;
    padding: 16px 18px;
    transition: border-color 0.2s;
  }
  .kpi:hover { border-color: var(--accent); }
  .kpi-label {
    font-size: 11px;
    color: var(--muted);
    text-transform: uppercase;
    letter-spacing: 1px;
    font-family: var(--mono);
    margin-bottom: 6px;
  }
  .kpi-value {
    font-size: 28px;
    font-weight: 700;
    font-family: var(--mono);
    color: var(--text);
    line-height: 1;
  }
  .kpi-value.accent  { color: var(--accent); }
  .kpi-value.success { color: var(--accent2); }
  .kpi-value.warn    { color: var(--warn); }
  .kpi-value.danger  { color: var(--danger); }
  .kpi-sub {
    font-size: 11px;
    color: var(--muted);
    margin-top: 4px;
    font-family: var(--mono);
  }

  /* ── Before/After comparison ── */
  .compare-grid {
    display: grid;
    grid-template-columns: 1fr auto 1fr;
    gap: 0;
    align-items: stretch;
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: 8px;
    overflow: hidden;
    margin-bottom: 24px;
  }
  .compare-panel { padding: 24px; }
  .compare-panel.before { border-right: 1px solid var(--border); }
  .compare-panel.after  { }
  .compare-arrow {
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0 16px;
    font-size: 24px;
    color: var(--accent);
    background: var(--surface2);
  }
  .compare-panel h3 {
    font-size: 11px;
    letter-spacing: 2px;
    text-transform: uppercase;
    color: var(--muted);
    font-family: var(--mono);
    margin-bottom: 16px;
  }
  .compare-row {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 6px 0;
    border-bottom: 1px solid rgba(48,54,61,0.5);
    font-size: 13px;
  }
  .compare-row:last-child { border-bottom: none; }
  .compare-key  { color: var(--muted); }
  .compare-val  { font-family: var(--mono); font-weight: 600; }
  .compare-val.green { color: var(--accent2); }
  .compare-val.blue  { color: var(--accent); }
  .compare-val.red   { color: var(--danger); }

  /* ── Per-file cards ── */
  .file-card {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: 8px;
    margin-bottom: 16px;
    overflow: hidden;
  }
  .file-card-header {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 14px 20px;
    background: var(--surface2);
    border-bottom: 1px solid var(--border);
    cursor: pointer;
    user-select: none;
  }
  .file-card-header:hover { background: #1f2937; }
  .file-icon { font-size: 18px; }
  .file-name { font-weight: 600; flex: 1; font-family: var(--mono); font-size: 13px; }
  .file-size { font-family: var(--mono); font-size: 12px; color: var(--muted); }
  .file-toggle { color: var(--muted); font-size: 12px; transition: transform 0.2s; }
  .file-toggle.open { transform: rotate(90deg); }
  .file-body { padding: 20px; display: none; }
  .file-body.open { display: block; }
  .file-stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(130px, 1fr));
    gap: 10px;
    margin-bottom: 20px;
  }
  .file-stat {
    background: var(--bg);
    border: 1px solid var(--border);
    border-radius: 5px;
    padding: 10px 14px;
  }
  .file-stat-label {
    font-size: 10px;
    color: var(--muted);
    text-transform: uppercase;
    letter-spacing: 1px;
    font-family: var(--mono);
    margin-bottom: 4px;
  }
  .file-stat-value {
    font-size: 20px;
    font-weight: 700;
    font-family: var(--mono);
  }

  /* ── Folder tree ── */
  .folder-tree { font-family: var(--mono); font-size: 12px; }
  .folder-row {
    display: flex;
    align-items: center;
    padding: 3px 0;
    color: var(--text);
  }
  .folder-indent { color: var(--border); margin-right: 4px; }
  .folder-icon { margin-right: 6px; }
  .folder-path { flex: 1; }
  .folder-count {
    color: var(--accent);
    font-weight: 600;
    margin-left: 8px;
    min-width: 50px;
    text-align: right;
  }
  .folder-emails { color: var(--accent2); margin-left: 8px; min-width: 60px; text-align: right; font-size: 11px; }

  /* ── Bar chart ── */
  .bar-chart { margin: 8px 0 24px; }
  .bar-row {
    display: flex;
    align-items: center;
    margin-bottom: 8px;
    gap: 10px;
  }
  .bar-label { width: 160px; font-size: 12px; color: var(--muted); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
  .bar-track { flex: 1; background: var(--surface2); border-radius: 3px; height: 20px; overflow: hidden; }
  .bar-fill { height: 100%; border-radius: 3px; transition: width 0.6s ease; display: flex; align-items: center; padding-left: 8px; }
  .bar-fill-count { font-size: 11px; font-family: var(--mono); color: rgba(255,255,255,0.8); }
  .bar-val { width: 70px; text-align: right; font-family: var(--mono); font-size: 12px; color: var(--text); }

  /* ── Duplicate table ── */
  .dup-table { width: 100%; border-collapse: collapse; font-size: 12px; }
  .dup-table th {
    text-align: left;
    padding: 8px 12px;
    background: var(--surface2);
    color: var(--muted);
    font-weight: 600;
    font-family: var(--mono);
    font-size: 11px;
    letter-spacing: 1px;
    text-transform: uppercase;
    border-bottom: 2px solid var(--border);
    position: sticky;
    top: 0;
  }
  .dup-table td {
    padding: 7px 12px;
    border-bottom: 1px solid rgba(48,54,61,0.6);
    color: var(--text);
    vertical-align: top;
  }
  .dup-table tr:hover td { background: var(--surface2); }
  .dup-table .mono { font-family: var(--mono); }
  .badge {
    display: inline-block;
    padding: 2px 7px;
    border-radius: 20px;
    font-size: 10px;
    font-weight: 600;
    font-family: var(--mono);
  }
  .badge-warn   { background: rgba(210,153,34,0.2);  color: var(--warn); }
  .badge-danger { background: rgba(248,81,73,0.2);   color: var(--danger); }
  .badge-info   { background: rgba(88,166,255,0.15); color: var(--accent); }
  .occ-pill {
    display: inline-block;
    padding: 2px 8px;
    background: var(--surface2);
    border: 1px solid var(--border);
    border-radius: 4px;
    font-family: var(--mono);
    font-size: 10px;
    margin: 2px 2px 2px 0;
    color: var(--muted);
  }

  /* ── Donut-style ring indicators (CSS only) ── */
  .ring-row { display: flex; gap: 24px; flex-wrap: wrap; margin-bottom: 20px; }
  .ring-item { text-align: center; }
  .ring-label { font-size: 11px; color: var(--muted); margin-top: 6px; font-family: var(--mono); }
  .ring-val   { font-size: 22px; font-weight: 700; font-family: var(--mono); margin-top: 2px; }

  /* ── Progress bar stat row ── */
  .stat-row {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 6px;
    font-size: 12px;
  }
  .stat-name { width: 120px; color: var(--muted); }
  .stat-bar  { flex: 1; height: 8px; background: var(--surface2); border-radius: 4px; overflow: hidden; }
  .stat-fill { height: 100%; border-radius: 4px; }
  .stat-cnt  { width: 60px; text-align: right; font-family: var(--mono); }

  /* ── Footer ── */
  .footer {
    border-top: 1px solid var(--border);
    padding: 20px 48px;
    color: var(--muted);
    font-size: 11px;
    font-family: var(--mono);
    display: flex;
    justify-content: space-between;
  }

  /* ── No-data state ── */
  .empty { text-align: center; padding: 40px; color: var(--muted); font-style: italic; }

  /* ── Scrollable wrapper for large tables ── */
  .table-wrap { max-height: 420px; overflow-y: auto; border: 1px solid var(--border); border-radius: 6px; }

  /* ── Diff indicator ── */
  .diff-pos { color: var(--accent2); font-family: var(--mono); font-size: 11px; }
  .diff-neg { color: var(--danger);  font-family: var(--mono); font-size: 11px; }
</style>
</head>
<body>");

            // ── Header ────────────────────────────────────────────────────────
            sb.AppendLine($@"
<div class=""header"">
  <div class=""header-tag"">DataGuardNXT &mdash; Enterprise PST Analytics</div>
  <h1>PST Merge Analysis Report</h1>
  <div class=""header-meta"">Generated: {report.ReportGeneratedAt:yyyy-MM-dd HH:mm:ss} &nbsp;|&nbsp; Source Files: {report.TotalSourceFiles}</div>
</div>
<div class=""container"">");

            // ── Executive KPIs ─────────────────────────────────────────────────
            sb.AppendLine(@"<div class=""section-title"">Executive Summary</div>");
            sb.AppendLine(@"<div class=""kpi-grid"">");
            KpiCard(sb, "Source PST Files",      report.TotalSourceFiles.ToString(),     "");
            KpiCard(sb, "Total Items (Before)",  report.TotalSourceItems.ToString("N0"), "", "accent");
            KpiCard(sb, "Total Emails",          report.TotalSourceEmails.ToString("N0"),"");
            KpiCard(sb, "Cross-File Duplicates", report.CrossFileDuplicates.Count.ToString("N0"), "", "warn");
            KpiCard(sb, "Total Dups Detected",   report.TotalDuplicatesRemoved.ToString("N0"), "", report.TotalDuplicatesRemoved > 0 ? "warn" : "success");
            KpiCard(sb, "Dedup Rate",            $"{report.DeduplicationRate:F1}%", "", report.DeduplicationRate > 20 ? "danger" : "success");
            KpiCard(sb, "Total Source Size",     report.TotalSourceSizeDisplay, "");
            if (report.MergedReport != null)
            {
                KpiCard(sb, "Items After Merge", report.ActualAfterMerge.ToString("N0"), "", "success");
                int saved = report.TotalSourceItems - report.ActualAfterMerge;
                KpiCard(sb, "Items Removed",     saved.ToString("N0"), "by deduplication", saved > 0 ? "accent2" : "");
            }
            sb.AppendLine("</div>");

            // ── Before / After comparison ──────────────────────────────────────
            if (report.MergedReport != null)
            {
                sb.AppendLine(@"<div class=""section-title"">Before vs After Merge</div>");
                sb.AppendLine(@"<div class=""compare-grid"">");

                // BEFORE
                sb.AppendLine(@"<div class=""compare-panel before""><h3>Before Merge (All Sources)</h3>");
                CompareRow(sb, "Total Items",     report.TotalSourceItems.ToString("N0"),  "blue");
                CompareRow(sb, "Emails",          report.TotalSourceEmails.ToString("N0"), "");
                CompareRow(sb, "Calendar",        report.SourceReports.Sum(r => r.TotalCalendar).ToString("N0"), "");
                CompareRow(sb, "Contacts",        report.SourceReports.Sum(r => r.TotalContacts).ToString("N0"), "");
                CompareRow(sb, "Tasks",           report.SourceReports.Sum(r => r.TotalTasks).ToString("N0"),    "");
                CompareRow(sb, "Total Folders",   report.SourceReports.Sum(r => r.TotalFolders).ToString("N0"), "");
                CompareRow(sb, "Source Files",    report.TotalSourceFiles.ToString(),   "");
                CompareRow(sb, "Total Size",      report.TotalSourceSizeDisplay,        "");
                sb.AppendLine("</div>");

                // ARROW
                sb.AppendLine(@"<div class=""compare-arrow"">&#8594;</div>");

                // AFTER
                var mr = report.MergedReport;
                int diff = mr.TotalItems - report.TotalSourceItems;
                sb.AppendLine(@"<div class=""compare-panel after""><h3>After Merge (Master PST)</h3>");
                CompareRow(sb, "Total Items",   mr.TotalItems.ToString("N0"),    diff <= 0 ? "green" : "red",
                               diff != 0 ? $" ({(diff > 0 ? "+" : "")}{diff:N0})" : "");
                CompareRow(sb, "Emails",        mr.TotalEmails.ToString("N0"),   "");
                CompareRow(sb, "Calendar",      mr.TotalCalendar.ToString("N0"), "");
                CompareRow(sb, "Contacts",      mr.TotalContacts.ToString("N0"), "");
                CompareRow(sb, "Tasks",         mr.TotalTasks.ToString("N0"),    "");
                CompareRow(sb, "Total Folders", mr.TotalFolders.ToString("N0"),  "");
                CompareRow(sb, "PST File",      "1 (master)",                    "");
                CompareRow(sb, "File Size",     mr.FileSizeDisplay,              "");
                sb.AppendLine("</div>");
                sb.AppendLine("</div>"); // compare-grid
            }

            // ── Per-file breakdown ─────────────────────────────────────────────
            sb.AppendLine(@"<div class=""section-title"">Per-File Analysis (Before Merge)</div>");

            int maxItems = report.SourceReports.Count > 0 ? report.SourceReports.Max(r => r.TotalItems) : 1;

            foreach (var fr in report.SourceReports)
            {
                string cardId = "fc_" + Math.Abs(fr.FileName.GetHashCode());
                sb.AppendLine($@"<div class=""file-card"">
  <div class=""file-card-header"" onclick=""toggleCard('{cardId}')"">
    <span class=""file-icon"">&#128230;</span>
    <span class=""file-name"">{HtmlEncode(fr.FileName)}</span>
    <span class=""file-size"">{fr.FileSizeDisplay}</span>");

                if (!fr.ScanSucceeded)
                    sb.AppendLine(@"    <span class=""badge badge-danger"">SCAN ERROR</span>");
                else if (fr.DuplicatesWithinFile + fr.DuplicatesFoundAcrossFiles > 0)
                    sb.AppendLine($@"    <span class=""badge badge-warn"">{fr.DuplicatesWithinFile + fr.DuplicatesFoundAcrossFiles} dups</span>");

                sb.AppendLine($@"    <span class=""file-toggle"" id=""toggle_{cardId}"">&#9654;</span>
  </div>
  <div class=""file-body"" id=""{cardId}"">");

                if (!fr.ScanSucceeded)
                {
                    sb.AppendLine($@"<div class=""empty"">⚠ Scan Error: {HtmlEncode(fr.ScanError)}</div>");
                }
                else
                {
                    // Mini KPI row
                    sb.AppendLine(@"<div class=""file-stats-grid"">");
                    FileStat(sb, "Total Items",   fr.TotalItems.ToString("N0"),    "var(--accent)");
                    FileStat(sb, "Emails",        fr.TotalEmails.ToString("N0"),   "var(--accent2)");
                    FileStat(sb, "Calendar",      fr.TotalCalendar.ToString("N0"), "var(--warn)");
                    FileStat(sb, "Contacts",      fr.TotalContacts.ToString("N0"), "var(--text)");
                    FileStat(sb, "Tasks",         fr.TotalTasks.ToString("N0"),    "var(--text)");
                    FileStat(sb, "Folders",       fr.TotalFolders.ToString("N0"),  "var(--muted)");
                    FileStat(sb, "Dups (within)", fr.DuplicatesWithinFile.ToString("N0"), "var(--danger)");
                    FileStat(sb, "Dups (cross)",  fr.DuplicatesFoundAcrossFiles.ToString("N0"), "var(--warn)");
                    sb.AppendLine("</div>");

                    // Item type distribution bar
                    sb.AppendLine(@"<div style=""margin-bottom:16px;""><div style=""font-size:11px;color:var(--muted);font-family:var(--mono);margin-bottom:8px;"">ITEM TYPE DISTRIBUTION</div>");
                    sb.AppendLine(@"<div class=""bar-chart"">");
                    BarRow(sb, "Emails",    fr.TotalEmails,    fr.TotalItems, "#58a6ff");
                    BarRow(sb, "Calendar",  fr.TotalCalendar,  fr.TotalItems, "#3fb950");
                    BarRow(sb, "Contacts",  fr.TotalContacts,  fr.TotalItems, "#d29922");
                    BarRow(sb, "Tasks",     fr.TotalTasks,     fr.TotalItems, "#bc8cff");
                    BarRow(sb, "Notes",     fr.TotalNotes,     fr.TotalItems, "#79c0ff");
                    BarRow(sb, "Other",     fr.TotalOther,     fr.TotalItems, "#8b949e");
                    sb.AppendLine("</div></div>");

                    // Folder tree
                    if (fr.RootFolder != null)
                    {
                        sb.AppendLine(@"<div style=""font-size:11px;color:var(--muted);font-family:var(--mono);margin-bottom:8px;"">FOLDER STRUCTURE</div>");
                        sb.AppendLine(@"<div class=""folder-tree"">");
                        RenderFolderTree(sb, fr.RootFolder, 0);
                        sb.AppendLine("</div>");
                    }
                }

                sb.AppendLine("</div></div>"); // file-body + file-card
            }

            // ── After-merge PST details ────────────────────────────────────────
            if (report.MergedReport != null)
            {
                var mr = report.MergedReport;
                sb.AppendLine(@"<div class=""section-title"">Merged PST — Post-Merge Analysis</div>");
                sb.AppendLine(@"<div class=""kpi-grid"">");
                KpiCard(sb, "Total Items",    mr.TotalItems.ToString("N0"),    "", "success");
                KpiCard(sb, "Emails",         mr.TotalEmails.ToString("N0"),   "");
                KpiCard(sb, "Calendar",       mr.TotalCalendar.ToString("N0"), "");
                KpiCard(sb, "Contacts",       mr.TotalContacts.ToString("N0"), "");
                KpiCard(sb, "Tasks",          mr.TotalTasks.ToString("N0"),    "");
                KpiCard(sb, "Total Folders",  mr.TotalFolders.ToString("N0"),  "");
                KpiCard(sb, "File Size",      mr.FileSizeDisplay,              "");
                sb.AppendLine("</div>");

                sb.AppendLine(@"<div style=""margin-top:16px;""><div class=""bar-chart"">");
                BarRow(sb, "Emails",   mr.TotalEmails,   mr.TotalItems, "#58a6ff");
                BarRow(sb, "Calendar", mr.TotalCalendar, mr.TotalItems, "#3fb950");
                BarRow(sb, "Contacts", mr.TotalContacts, mr.TotalItems, "#d29922");
                BarRow(sb, "Tasks",    mr.TotalTasks,    mr.TotalItems, "#bc8cff");
                BarRow(sb, "Notes",    mr.TotalNotes,    mr.TotalItems, "#79c0ff");
                BarRow(sb, "Other",    mr.TotalOther,    mr.TotalItems, "#8b949e");
                sb.AppendLine("</div></div>");

                if (mr.RootFolder != null)
                {
                    sb.AppendLine(@"<div style=""font-size:11px;color:var(--muted);font-family:var(--mono);margin:16px 0 8px;"">MERGED PST FOLDER STRUCTURE</div>");
                    sb.AppendLine(@"<div class=""folder-tree"">");
                    RenderFolderTree(sb, mr.RootFolder, 0);
                    sb.AppendLine("</div>");
                }
            }

            // ── Cross-file duplicates table ────────────────────────────────────
            sb.AppendLine(@"<div class=""section-title"">Cross-File Duplicate Analysis</div>");

            if (report.CrossFileDuplicates.Count == 0)
            {
                sb.AppendLine(@"<div class=""empty"">&#10003; No cross-file duplicates detected.</div>");
            }
            else
            {
                sb.AppendLine($@"<p style=""color:var(--muted);font-size:12px;margin-bottom:12px;"">
  {report.CrossFileDuplicates.Count} duplicate group(s) found across source files.
  Removing them would eliminate <strong style=""color:var(--warn)"">{report.CrossFileDuplicates.Sum(g => g.Count - 1)}</strong> redundant item(s).
</p>");
                sb.AppendLine(@"<div class=""table-wrap""><table class=""dup-table"">
<thead><tr>
  <th>#</th><th>Subject</th><th>Copies</th><th>Found In</th>
</tr></thead><tbody>");

                int row = 0;
                foreach (var grp in report.CrossFileDuplicates.OrderByDescending(g => g.Count).Take(500))
                {
                    row++;
                    string subject = string.IsNullOrEmpty(grp.Subject) ? "(no subject)" : grp.Subject;
                    string badgeCls = grp.Count >= 4 ? "badge-danger" : "badge-warn";

                    sb.AppendLine($@"<tr>
  <td class=""mono"" style=""color:var(--muted)"">{row}</td>
  <td>{HtmlEncode(subject)}</td>
  <td><span class=""badge {badgeCls}"">{grp.Count}x</span></td>
  <td>");
                    var fileGroups = grp.Occurrences
                        .GroupBy(o => o.SourceFile)
                        .Select(g => $"{HtmlEncode(g.Key)} ({g.Count()})");
                    foreach (var fg in fileGroups)
                        sb.AppendLine($@"<span class=""occ-pill"">{fg}</span>");
                    sb.AppendLine("</td></tr>");
                }

                if (report.CrossFileDuplicates.Count > 500)
                    sb.AppendLine($@"<tr><td colspan=""4"" style=""text-align:center;color:var(--muted);padding:12px;"">
      ... and {report.CrossFileDuplicates.Count - 500} more groups not shown.
    </td></tr>");

                sb.AppendLine("</tbody></table></div>");
            }

            // ── Within-file duplicates summary ────────────────────────────────
            var filesWithInternalDups = report.SourceReports.Where(r => r.DuplicatesWithinFile > 0).ToList();
            if (filesWithInternalDups.Any())
            {
                sb.AppendLine(@"<div class=""section-title"">Within-File Duplicate Summary</div>");
                sb.AppendLine(@"<div class=""bar-chart"">");
                int maxDup = filesWithInternalDups.Max(r => r.DuplicatesWithinFile);
                foreach (var fr in filesWithInternalDups.OrderByDescending(r => r.DuplicatesWithinFile))
                    BarRow(sb, fr.FileName, fr.DuplicatesWithinFile, maxDup > 0 ? maxDup : 1, "#d29922");
                sb.AppendLine("</div>");
            }

            // ── Items per source file (comparison chart) ───────────────────────
            sb.AppendLine(@"<div class=""section-title"">Items per Source File (Comparison)</div>");
            sb.AppendLine(@"<div class=""bar-chart"">");
            int globalMax = report.SourceReports.Count > 0 ? report.SourceReports.Max(r => r.TotalItems) : 1;
            foreach (var fr in report.SourceReports.OrderByDescending(r => r.TotalItems))
                BarRow(sb, fr.FileName, fr.TotalItems, globalMax > 0 ? globalMax : 1, "#58a6ff");
            sb.AppendLine("</div>");

            // ── Close container + JS ──────────────────────────────────────────
            sb.AppendLine(@"</div>"); // container

            sb.AppendLine($@"
<div class=""footer"">
  <span>PST Analyzer &mdash; DataGuardNXT 2026</span>
  <span>Report ID: {Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()}</span>
</div>

<script>
function toggleCard(id) {{
  var body = document.getElementById(id);
  var tog  = document.getElementById('toggle_' + id);
  if (body.classList.contains('open')) {{
    body.classList.remove('open');
    tog.classList.remove('open');
  }} else {{
    body.classList.add('open');
    tog.classList.add('open');
  }}
}}
// Auto-open first card if only one file
(function() {{
  var cards = document.querySelectorAll('.file-body');
  if (cards.length === 1) {{
    cards[0].classList.add('open');
    var t = document.querySelectorAll('.file-toggle')[0];
    if (t) t.classList.add('open');
  }}
}})();
</script>
</body></html>");

            return sb.ToString();
        }

        // ── HTML helpers ──────────────────────────────────────────────────────

        private static void KpiCard(StringBuilder sb, string label, string value, string sub, string valueClass = "")
        {
            sb.AppendLine($@"<div class=""kpi"">
  <div class=""kpi-label"">{label}</div>
  <div class=""kpi-value {valueClass}"">{value}</div>
  {(string.IsNullOrEmpty(sub) ? "" : $@"<div class=""kpi-sub"">{sub}</div>")}
</div>");
        }

        private static void FileStat(StringBuilder sb, string label, string value, string color)
        {
            sb.AppendLine($@"<div class=""file-stat"">
  <div class=""file-stat-label"">{label}</div>
  <div class=""file-stat-value"" style=""color:{color}"">{value}</div>
</div>");
        }

        private static void CompareRow(StringBuilder sb, string key, string val, string cls, string extra = "")
        {
            sb.AppendLine($@"<div class=""compare-row"">
  <span class=""compare-key"">{key}</span>
  <span class=""compare-val {cls}"">{val}{extra}</span>
</div>");
        }

        private static void BarRow(StringBuilder sb, string label, int value, int max, string color)
        {
            double pct = max > 0 ? Math.Min(100.0, (value / (double)max) * 100.0) : 0;
            string shortLabel = label.Length > 22 ? label.Substring(0, 20) + "…" : label;
            sb.AppendLine($@"<div class=""bar-row"">
  <div class=""bar-label"" title=""{HtmlEncode(label)}"">{HtmlEncode(shortLabel)}</div>
  <div class=""bar-track"">
    <div class=""bar-fill"" style=""width:{pct:F1}%;background:{color};"">
      <span class=""bar-fill-count"">{(pct > 12 ? value.ToString("N0") : "")}</span>
    </div>
  </div>
  <div class=""bar-val"">{value:N0}</div>
</div>");
        }

        private static void RenderFolderTree(StringBuilder sb, FolderStats folder, int depth)
        {
            if (folder == null) return;
            string indent = new string(' ', depth * 4).Replace(" ", "&nbsp;");
            string icon   = depth == 0 ? "&#128230;" : "&#128193;";
            sb.AppendLine($@"<div class=""folder-row"">
  <span class=""folder-indent"">{indent}</span>
  <span class=""folder-icon"">{icon}</span>
  <span class=""folder-path"">{HtmlEncode(folder.FolderName)}</span>
  <span class=""folder-count"">{folder.TotalItems:N0}</span>
  <span class=""folder-emails"">{(folder.EmailItems > 0 ? folder.EmailItems.ToString("N0") + " mail" : "")}</span>
</div>");
            foreach (var sub in folder.SubFolders.OrderByDescending(f => f.TotalItems))
                RenderFolderTree(sb, sub, depth + 1);
        }

        private static string HtmlEncode(string s)
        {
            if (s == null) return "";
            return s.Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;");
        }
    }
}
