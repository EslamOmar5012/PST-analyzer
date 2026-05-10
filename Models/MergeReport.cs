using System;
using System.Collections.Generic;
using System.Linq;

namespace PstAnalyzer.Models
{
    /// <summary>
    /// Aggregated report covering all source PSTs plus after-merge analysis.
    /// </summary>
    public class MergeReport
    {
        // ── Source PST reports (before merge) ─────────────────────────────────
        public List<PstFileReport> SourceReports { get; set; } = new List<PstFileReport>();

        // ── Merged PST report (after merge) ───────────────────────────────────
        public PstFileReport MergedReport { get; set; }

        // ── Cross-file duplicate groups ───────────────────────────────────────
        public List<DuplicateGroup> CrossFileDuplicates { get; set; } = new List<DuplicateGroup>();

        // ── Report metadata ───────────────────────────────────────────────────
        public DateTime ReportGeneratedAt { get; set; } = DateTime.Now;
        public string ReportTitle { get; set; } = "PST Merge Analysis Report";

        // ── Computed summaries ────────────────────────────────────────────────
        public int TotalSourceItems   => SourceReports.Sum(r => r.TotalItems);
        public int TotalSourceEmails  => SourceReports.Sum(r => r.TotalEmails);
        public int TotalSourceFiles   => SourceReports.Count;
        public long TotalSourceSize   => SourceReports.Sum(r => r.FileSizeBytes);

        public int TotalDuplicatesRemoved =>
            CrossFileDuplicates.Sum(g => g.Count - 1) +
            SourceReports.Sum(r => r.DuplicatesWithinFile);

        public int ExpectedAfterDedup =>
            TotalSourceItems - TotalDuplicatesRemoved;

        public int ActualAfterMerge =>
            MergedReport != null ? MergedReport.TotalItems : -1;

        public double DeduplicationRate =>
            TotalSourceItems > 0
                ? (TotalDuplicatesRemoved / (double)TotalSourceItems) * 100.0
                : 0;

        public string TotalSourceSizeDisplay =>
            TotalSourceSize >= 1_073_741_824
                ? string.Format("{0:F2} GB", TotalSourceSize / 1_073_741_824.0)
                : string.Format("{0:F2} MB", TotalSourceSize / 1_048_576.0);
    }
}
