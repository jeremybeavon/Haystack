using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IHaystackConfiguration
    {
        string HaystackBaseDirectory { get; }

        string OutputDirectory { get; }

        IAmendmentConfiguration Amendments { get; }

        IEnumerable<ICodeCoverageConfiguration> CodeCoverage { get; }

        IEnumerable<IInterceptionConfiguration> Interception { get; }

        IEnumerable<IStaticAnalysisConfiguration> StaticAnalysis { get; }

        IEnumerable<ISourceControlConfiguration> SourceControl { get; }

        IRunnerConfiguration Runner { get; }
    }
}
