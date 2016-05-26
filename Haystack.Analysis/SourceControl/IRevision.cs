using System;

namespace Haystack.Analysis.SourceControl
{
    public interface IRevision
    {
        string Revision { get; }

        string Author { get; }

        DateTimeOffset Date { get; }
    }
}
