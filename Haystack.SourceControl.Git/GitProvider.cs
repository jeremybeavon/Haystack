using Haystack.Diagnostics.SourceControl;
using System;
using System.Collections.Generic;

namespace Haystack.SourceControl.Git
{
    public sealed class GitProvider : ISourceControlProvider
    {
        public IEnumerable<ILineChange> Blame(IPartialFile file, string startRevision, string endRevision)
        {
            throw new NotImplementedException();
        }
    }
}
