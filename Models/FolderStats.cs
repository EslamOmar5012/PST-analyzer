using System.Collections.Generic;

namespace PstAnalyzer.Models
{
    /// <summary>
    /// Statistics for a single folder inside a PST file.
    /// </summary>
    public class FolderStats
    {
        public string FolderPath { get; set; }   // e.g. "Inbox\Projects"
        public string FolderName { get; set; }
        public int TotalItems { get; set; }
        public int EmailItems { get; set; }
        public int CalendarItems { get; set; }
        public int ContactItems { get; set; }
        public int TaskItems { get; set; }
        public int NoteItems { get; set; }
        public int OtherItems { get; set; }
        public long TotalSizeBytes { get; set; }
        public List<FolderStats> SubFolders { get; set; } = new List<FolderStats>();
    }
}
