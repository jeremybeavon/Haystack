using MsgPack.Serialization;
using System.Collections.Generic;
using System;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class CodeCoverageNestedClass : ICodeCoverageNestedClass
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

        public CodeCoverageNestedClass NestedClass { get; set; }

        IEnumerable<ICodeCoverageMethod> ICodeCoverageNestedClass.Methods
        {
            get { return Methods; }
        }

        IEnumerable<ICodeCoverageNestedClass> ICodeCoverageNestedClass.NestedClasses
        {
            get { return NestedClasses; }
        }

        ICodeCoverageClass ICodeCoverageNestedClass.Class
        {
            get { return Class; }
        }
    }
}
