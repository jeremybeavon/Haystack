using MsgPack.Serialization;
using System.Collections.Generic;
using System.Threading;
using System;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class CodeCoverageMethod : ICodeCoverageMethod
    {
        private static int nextCodeCoverageMethodId;

        public CodeCoverageMethod()
        {
            CodeCoverageMethodId = Interlocked.Increment(ref nextCodeCoverageMethodId);
        }

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

        IEnumerable<ICodeCoverageMethodParameter> ICodeCoverageMethod.MethodParameters
        {
            get { return MethodParameters; }
        }

        IEnumerable<ICodeCoverageLine> ICodeCoverageMethod.Lines
        {
            get { return Lines; }
        }

        ICodeCoverageClass ICodeCoverageMethod.Class
        {
            get { return Class; }
        }

        ICodeCoverageNestedClass ICodeCoverageMethod.NestedClass
        {
            get { return NestedClass; }
        }
    }
}
