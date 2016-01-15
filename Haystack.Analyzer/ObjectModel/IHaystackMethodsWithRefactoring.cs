using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public interface IHaystackMethodsWithRefactoring
    {
        IEnumerable<IRefactoredMethod> RefactoredMethods { get; }

        IEnumerable<IHaystackMethod> NonRefactoredMethods { get; }
    }
}
