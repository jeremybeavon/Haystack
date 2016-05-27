using System.Collections.Generic;

namespace Haystack.Analysis.Configuration
{
    public interface IHaystackAnalysisConfiguration
    {
        string HaystackBaseDirectory { get; }

        string HaystackAnalysisDirectory { get; }

        string PassingTestOutputDirectory { get; }

        string FailingTestOutputDirectory { get; }

        string OutputDirectory { get; }

        IEnumerable<ICodeCoverageConfiguration> CodeCoverage { get; }

        IEnumerable<ISourceControlConfiguration> SourceControl { get; }
    }
}
