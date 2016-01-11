using MsgPack.Serialization;
using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageMethod
    {
        [MessagePackMember(0)]
        public int CodeCoverageMethodId { get; set; }

        [MessagePackMember(1)]
        public string MethodName { get; set; }

        [MessagePackMember(2)]
        public List<CodeCoverageMethodParameter> MethodParameters { get; set; }

        [MessagePackMember(3)]
        public string ReturnType { get; set; }

        [MessagePackMember(4)]
        public bool HasCodeCoverage { get; set; }

        [MessagePackMember(5)]
        public List<CodeCoverageLine> Lines { get; set; }

        public CodeCoverageClass Class { get; set; }

        public CodeCoverageNestedClass NestedClass { get; set; }
    }
}
