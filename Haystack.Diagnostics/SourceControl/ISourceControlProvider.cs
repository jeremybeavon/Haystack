using System.Collections.Generic;

namespace Haystack.Diagnostics.SourceControl
{
    public interface ISourceControlProvider
    {
        IEnumerable<ILineChange> Blame(IPartialFile file, string startRevision, string endRevision);
    }
}
