using System;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class SourceControlRevision : ISourceControlRevision
    {
        public string Revision { get; set; }

        public string Author { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
