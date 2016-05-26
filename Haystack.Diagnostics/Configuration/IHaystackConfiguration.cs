using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IHaystackConfiguration
    {
        string HaystackBaseDirectory { get; }

        string HaystackRunnerDirectory { get; }

        string HaystackDiagnosticsDirectory { get; }

        string OutputDirectory { get; }

        IAmendmentConfiguration Amendments { get; }

        IEnumerable<ICodeCoverageConfiguration> CodeCoverage { get; }

        IEnumerable<IInterceptionConfiguration> Interception { get; }

        IEnumerable<IStaticAnalysisConfiguration> StaticAnalysis { get; }

        IRunnerConfiguration Runner { get; }
    }
}
