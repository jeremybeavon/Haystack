using System;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class SourceControlRevision : ISourceControlRevision
    {
        public string Revision { get; set; }

        public string Author { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
