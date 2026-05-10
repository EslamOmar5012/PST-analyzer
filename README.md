# PST Analyzer & Report Tool
### DataGuardNXT 2026 — Standalone Companion to PstMerger

---

## Overview

This is a **completely separate project** from PstMerger. It does **not modify** any PST files.
It connects to Outlook (read-only), scans your PST files, and generates a rich **HTML analytics report**.

---

## What the Report Shows

### Before Merge
- Per-file item counts (Total, Emails, Calendar, Contacts, Tasks, Notes, Other)
- File size and last-modified date
- Full folder tree with per-folder item counts
- Bar chart of item type distribution
- Within-file duplicate detection (items duplicated inside the same PST)
- Cross-file duplicate detection (same item appearing in multiple PST files)

### After Merge (optional)
- Scan the already-merged master PST
- Side-by-side Before vs. After comparison
- Total items removed by deduplication
- Deduplication rate (%)
- Folder structure of the merged file

### Analytics
- Executive KPI dashboard
- Cross-file duplicate table (sortable, shows which files/folders contain each duplicate)
- Within-file duplicate summary chart
- Items-per-source-file comparison bar chart

---

## Requirements

- **Visual Studio 2019 / 2022 / 2026** (any edition)
- **.NET Framework 4.8**
- **Microsoft Outlook** installed (2013 / 2016 / 2019 / 365)
  — The tool uses `Microsoft.Office.Interop.Outlook` (same as PstMerger)
- The **Outlook Interop DLL** path in the `.csproj` is the same as PstMerger:
  `C:\Windows\assembly\GAC_MSIL\Microsoft.Office.Interop.Outlook\15.0.0.0__...`
  Adjust the `<HintPath>` in `PstAnalyzer.csproj` if your Outlook version differs.

---

## Project Structure

```
PstAnalyzer/
├── PstAnalyzer.csproj
├── Program.cs
├── Properties/
│   └── AssemblyInfo.cs
├── Models/
│   ├── FolderStats.cs        ← per-folder statistics
│   ├── DuplicateGroup.cs     ← a group of duplicate items
│   ├── PstFileReport.cs      ← full report for one PST file
│   └── MergeReport.cs        ← aggregated report (all files + merged)
├── Services/
│   ├── PstAnalyzerService.cs ← Outlook scanning logic (READ-ONLY)
│   └── HtmlReportGenerator.cs← generates self-contained HTML report
└── Forms/
    ├── MainAnalyzerForm.cs / .Designer.cs   ← main UI
    └── ReportViewerForm.cs / .Designer.cs   ← optional in-app viewer
```

---

## How to Use

1. **Open** `PstAnalyzer.csproj` in Visual Studio
2. **Build** (Ctrl+Shift+B) — restore references if prompted
3. **Run** (F5)
4. In the tool:
   - Click **Add Files...** or **Add Folder...** to add your source PST files
   - Optionally tick **Include in report** and browse to the already-merged PST
   - Set a save path for the HTML report (defaults to Desktop)
   - Click **▶ Run Analysis**
5. The HTML report opens automatically in your browser

---

## Duplicate Detection Algorithm

Uses the **identical MD5 hash algorithm as PstMerger** so results are directly comparable:

```
Hash = MD5(Subject | SenderName | SentOn | Size | BodyLength)
```

This means:
- A "duplicate" here = exactly what PstMerger would skip with Remove Duplicates ON
- The deduplication rate shown is an accurate preview of what PstMerger will remove

---

## Notes

- Analysis is **read-only** — no PST files are modified
- Large PSTs (10 GB+) may take several minutes to scan
- The HTML report is fully self-contained (no internet required to view)
- Run analysis **before** merging to preview duplicates
- Run analysis **after** merging (with the merged PST selected) to confirm results
