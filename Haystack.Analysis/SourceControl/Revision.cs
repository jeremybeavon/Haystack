using System;

namespace Haystack.Analysis.SourceControl
{
    public sealed class Revision : IRevision
    {
        private readonly string revision;

        public Revision(string revision, string author, DateTimeOffset date)
        {
            this.revision = revision;
            Author = author;
            Date = date;
        }

        public string Author { get; private set; }

        public DateTimeOffset Date { get; private set; }

        string IRevision.Revision
        {
            get { return revision; }
        }
    }
}
