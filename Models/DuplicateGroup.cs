using System.Collections.Generic;

namespace PstAnalyzer.Models
{
    /// <summary>
    /// Represents a group of items that are considered duplicates of each other.
    /// </summary>
    public class DuplicateGroup
    {
        public string Hash { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string SentOn { get; set; }
        public int Count { get; set; }                        // how many copies exist
        public List<DuplicateOccurrence> Occurrences { get; set; } = new List<DuplicateOccurrence>();
    }

    public class DuplicateOccurrence
    {
        public string SourceFile { get; set; }   // which PST file
        public string FolderPath { get; set; }   // folder inside the PST
    }
}
