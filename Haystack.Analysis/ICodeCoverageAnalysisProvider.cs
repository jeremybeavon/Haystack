using System.Collections.Generic;
using Haystack.Analysis.Configuration;
using Haystack.Analysis.ObjectModel;

namespace Haystack.Analysis
{
    public interface ICodeCoverageAnalysisProvider
    {
        IEnumerable<CodeCoverageAnalysis> AnalyzeCodeCoverage(IHaystackAnalysisConfiguration configuration);
    }
}
