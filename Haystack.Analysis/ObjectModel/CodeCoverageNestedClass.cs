using MsgPack.Serialization;
using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageNestedClass
    {
        public CodeCoverageNestedClass()
        {
            Methods = new List<CodeCoverageMethod>();
            NestedClasses = new List<CodeCoverageNestedClass>();
        }

        [MessagePackMember(0)]
        public string ClassName { get; set; }

        [MessagePackMember(1)]
        public List<CodeCoverageMethod> Methods { get; set; }

        [MessagePackMember(2)]
        public List<CodeCoverageNestedClass> NestedClasses { get; set; }

        public CodeCoverageClass Class { get; set; }
    }
}
