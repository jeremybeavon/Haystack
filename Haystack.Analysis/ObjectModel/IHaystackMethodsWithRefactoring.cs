using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public interface IHaystackMethodsWithRefactoring
    {
        IEnumerable<IRefactoredMethod> RefactoredMethods { get; }

        IEnumerable<IHaystackMethod> NonRefactoredMethods { get; }
    }
}
