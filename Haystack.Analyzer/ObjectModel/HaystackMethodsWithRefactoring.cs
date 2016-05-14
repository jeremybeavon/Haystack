using MsgPack.Serialization;
using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class HaystackMethodsWithRefactoring : IHaystackMethodsWithRefactoring
    {
        public HaystackMethodsWithRefactoring()
        {
            RefactoredMethods = new List<RefactoredMethod>();
            NonRefactoredMethodIndexes = new List<int>();
            NonRefactoredMethods = new List<HaystackMethod>();
        }

        [MessagePackMember(0)]
        public List<RefactoredMethod> RefactoredMethods { get; set; }

        [MessagePackMember(1)]
        public List<int> NonRefactoredMethodIndexes { get; set; }

        public List<HaystackMethod> NonRefactoredMethods { get; set; }

        IEnumerable<IRefactoredMethod> IHaystackMethodsWithRefactoring.RefactoredMethods
        {
            get { return RefactoredMethods; }
        }

        IEnumerable<IHaystackMethod> IHaystackMethodsWithRefactoring.NonRefactoredMethods
        {
            get { return NonRefactoredMethods; }
        }
    }
}
