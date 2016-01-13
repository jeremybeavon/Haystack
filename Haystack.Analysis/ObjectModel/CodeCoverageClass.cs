using MsgPack.Serialization;
using System.Collections.Generic;
using System;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageClass : ICodeCoverageClass
    {
        public CodeCoverageClass()
        {
            Methods = new List<CodeCoverageMethod>();
            NestedClasses = new List<CodeCoverageNestedClass>();
        }

        [MessagePackMember(0)]
        public string NamespaceName { get; set; }

        [MessagePackMember(1)]
        public string ClassName { get; set; }

        [MessagePackMember(2)]
        public List<CodeCoverageMethod> Methods { get; set; }

        [MessagePackMember(3)]
        public List<CodeCoverageNestedClass> NestedClasses { get; set; }

        public CodeCoverageClassFile File { get; set; }

        IEnumerable<ICodeCoverageMethod> ICodeCoverageClass.Methods
        {
            get { return Methods; }
        }

        IEnumerable<ICodeCoverageNestedClass> ICodeCoverageClass.NestedClasses
        {
            get { return NestedClasses; }
        }

        ICodeCoverageClassFile ICodeCoverageClass.File
        {
            get { return File; }
        }
    }
}
