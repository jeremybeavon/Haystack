using Haystack.Diagnostics.ObjectModel;
using MsgPack.Serialization;
using System.Collections.Generic;
using System;

namespace Haystack.Analysis.ObjectModel
{
    public class HaystackMethod : IHaystackMethod
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
        public string ClassName { get; set; }

        [MessagePackMember(1)]
        public string MethodName { get; set; }

        [MessagePackMember(2)]
        public List<HaystackMethodParameter> MethodParameters { get; set; }

        [MessagePackMember(3)]
        public string ReturnType { get; set; }

        [MessagePackMember(4)]
        public CodeCoverageMethodDifferenceType CodeCoverageDifference { get; set; }

        [MessagePackMember(5)]
        public int? PassingMethodCoverageMethodId { get; set; }

        public CodeCoverageMethod PassingCodeCoverageMethod { get; set; }

        [MessagePackMember(6)]
        public int? FailingMethodCoverageMethodId { get; set; }

        public CodeCoverageMethod FailingCodeCoverageMethod { get; set; }

        [MessagePackMember(7)]
        public List<int> PassingMethodCallIds { get; set; }

        public List<MethodCall> PassingMethodCalls { get; set; }

        [MessagePackMember(8)]
        public List<int> FailingMethodCallIds { get; set; }

        public List<MethodCall> FailingMethodCalls { get; set; }

        [MessagePackMember(9)]
        public List<SourceControlLineChange> SourceControlChanges { get; set; }

        IEnumerable<IHaystackMethodParameter> IHaystackMethod.MethodParameters
        {
            get { return MethodParameters; }
        }
        
        ICodeCoverageMethod IHaystackMethod.PassingCodeCoverageMethod
        {
            get { return PassingCodeCoverageMethod; }
        }

        ICodeCoverageMethod IHaystackMethod.FailingCodeCoverageMethod
        {
            get { return FailingCodeCoverageMethod; }
        }

        IEnumerable<IMethodCall> IHaystackMethod.PassingMethodCalls
        {
            get { return PassingMethodCalls; }
        }

        IEnumerable<IMethodCall> IHaystackMethod.FailingMethodCalls
        {
            get { return FailingMethodCalls; }
        }

        public IEnumerable<ISourceControlLineChange> SourceControlLineChanges
        {
            get { return SourceControlChanges; }
        }
    }
}
