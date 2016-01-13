using System;
using MsgPack.Serialization;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class RefactoredMethod : HaystackMethod, IRefactoredMethod
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
    }
}
