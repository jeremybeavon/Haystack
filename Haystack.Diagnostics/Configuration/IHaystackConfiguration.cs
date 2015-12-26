using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IHaystackConfiguration
    {
        IAmendmentConfiguration Amendments { get; }

        IEnumerable<IInterceptionConfiguration> Interception { get; }

        IEnumerable<IStaticAnalysisConfiguration> StaticAnalysis { get; }

        IEnumerable<ISourceControlConfiguration> SourceControl { get; }

        IRunnerConfiguration Runner { get; }
    }
}
