using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class SourceControlLineChange
    {
        [MessagePackMember(0)]
        public int Line { get; set; }

        [MessagePackMember(1)]
        public int RevisionIndex { get; set; }

        public SourceControlRevision Revision { get; set; }
    }
}
