using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public interface IHaystackAnalysis
    {
        IEnumerable<ICodeCoverageAnalysis> CodeCoverageAnalysis { get; }

        IEnumerable<IMethodCallTraceFileAnalysis> MethodCallTraceFileAnalysis { get; }

        IEnumerable<ISourceControlRevision> SourceControlRevisions { get; }

        IEnumerable<IHaystackMethod> HaystackMethods { get; }

        IHaystackMethodsWithRefactoring HaystackMethodsWithRefactoring { get; }
    }
}
