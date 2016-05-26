using System.Collections.Generic;

namespace Haystack.Analysis.Configuration
{
    public interface IHaystackAnalysisConfiguration
    {
        string HaystackBaseDirectory { get; }

        string HaystackAnalysisDirectory { get; }

        string PassingTestOutputDirectory { get; }

        string FailingTestOutputDirectory { get; }

        IEnumerable<ICodeCoverageConfiguration> CodeCoverage { get; }

        IEnumerable<ISourceControlConfiguration> SourceControl { get; }
    }
}
