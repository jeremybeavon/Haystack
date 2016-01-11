using MsgPack.Serialization;
using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageNestedClass
    {
        [MessagePackMember(0)]
        public string ClassName { get; set; }

        [MessagePackMember(1)]
        public List<CodeCoverageMethod> Methods { get; set; }

        public CodeCoverageClass Class { get; set; }

        public CodeCoverageNestedClass NestedClass { get; set; }
    }
}
