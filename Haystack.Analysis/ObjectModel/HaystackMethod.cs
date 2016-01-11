using Haystack.Diagnostics.ObjectModel;
using MsgPack.Serialization;
using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public class HaystackMethod
    {
        public HaystackMethod()
        {
            MethodParameters = new List<HaystackMethodParameter>();
            PassingMethodCallIds = new List<int>();
            PassingMethodCalls = new List<MethodCall>();
            FailingMethodCallIds = new List<int>();
            FailingMethodCalls = new List<MethodCall>();
        }

        [MessagePackMember(0)]
        public string MethodName { get; set; }

        [MessagePackMember(1)]
        public string TypeName { get; set; }

        [MessagePackMember(2)]
        public List<HaystackMethodParameter> MethodParameters { get; set; }

        [MessagePackMember(3)]
        public string ReturnType { get; set; }

        [MessagePackMember(4)]
        public CodeCoverageMethodDifferenceType CodeCoverageDifference { get; set; }

        [MessagePackMember(5)]
        public int PassingMethodCoverageMethodId { get; set; }

        public CodeCoverageMethod PassingMethodCoverageMethod { get; set; }

        [MessagePackMember(6)]
        public int FailingMethodCoverageMethodId { get; set; }

        public CodeCoverageMethod FailingMethodCoverageMethod { get; set; }

        [MessagePackMember(7)]
        public List<int> PassingMethodCallIds { get; set; }

        public List<MethodCall> PassingMethodCalls { get; set; }

        [MessagePackMember(8)]
        public List<int> FailingMethodCallIds { get; set; }

        public List<MethodCall> FailingMethodCalls { get; set; }

        [MessagePackMember(9)]
        public List<SourceControlLineChange> SourceControlChanges { get; set; }
    }
}
