using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IStaticAnalysisConfiguration
    {
        string StaticAnalysisProvider { get; }

        IStaticAnalysis StaticAnalysisRunner { get; }

        string Name { get; }

        IEnumerable<string> IncludedItems { get; }

        IEnumerable<string> ExcludedItems { get; }
    }
}
