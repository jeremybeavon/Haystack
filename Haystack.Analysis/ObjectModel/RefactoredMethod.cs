using System;
using System.Collections.Generic;
using Haystack.Diagnostics.ObjectModel;
using MsgPack.Serialization;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class RefactoredMethod : IRefactoredMethod
    {
        [MessagePackMember(0)]
        public int PassingMethodIndex { get; set; }

        public HaystackMethod PassingMethod { get; set; }

        [MessagePackMember(1)]
        public int FailingMethodIndex { get; set; }

        public HaystackMethod FailingMethod { get; set; }

        [MessagePackMember(2)]
        public MethodRefactorTypes RefactorTypes { get; set; }

        IHaystackMethod IRefactoredMethod.PassingMethod
        {
            get { return PassingMethod; }
        }

        IHaystackMethod IRefactoredMethod.FailingMethod
        {
            get { return FailingMethod; }
        }

        public string ClassName
        {
            get { return FailingMethod.ClassName; }
        }

        public string MethodName
        {
            get { return FailingMethod.MethodName; }
        }

        public IEnumerable<IHaystackMethodParameter> MethodParameters
        {
            get { return FailingMethod.MethodParameters; }
        }

        public CodeCoverageMethodDifferenceType CodeCoverageDifference
        {
            get { throw new NotImplementedException(); }
        }

        public ICodeCoverageMethod PassingCodeCoverageMethod
        {
            get { return PassingMethod.PassingCodeCoverageMethod; }
        }

        public ICodeCoverageMethod FailingCodeCoverageMethod
        {
            get { return FailingMethod.FailingCodeCoverageMethod; }
        }

        public IEnumerable<IMethodCall> PassingMethodCalls
        {
            get { return PassingMethod.PassingMethodCalls; }
        }

        public IEnumerable<IMethodCall> FailingMethodCalls
        {
            get { return FailingMethod.FailingMethodCalls; }
        }

        public IEnumerable<ISourceControlLineChange> SourceControlLineChanges
        {
            get { return FailingMethod.SourceControlLineChanges; }
        }
    }
}
