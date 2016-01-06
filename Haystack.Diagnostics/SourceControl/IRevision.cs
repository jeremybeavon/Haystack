using System;

namespace Haystack.Diagnostics.SourceControl
{
    public interface IRevision
    {
        string Revision { get; }

        string Author { get; }

        DateTimeOffset Date { get; }
    }
}
