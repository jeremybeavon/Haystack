using System.Collections.Generic;

namespace Haystack.Diagnostics.SourceControl
{
    public interface ISourceControlProvider
    {
        IEnumerable<ILineChange> Blame(string file, string startRevision, string endRevision);
    }
}
