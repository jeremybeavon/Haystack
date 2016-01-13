using System;
using MsgPack.Serialization;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class SourceControlLineChange : ISourceControlLineChange
    {
        [MessagePackMember(0)]
        public int Line { get; set; }

        [MessagePackMember(1)]
        public int RevisionIndex { get; set; }

        public SourceControlRevision Revision { get; set; }

        ISourceControlRevision ISourceControlLineChange.Revision
        {
            get { return Revision; }
        }
    }
}
