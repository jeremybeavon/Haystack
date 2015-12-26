using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IStaticAnalysisConfiguration
    {
        string StaticAnalysisFramework { get; }

        IEnumerable<string> IncludedItems { get; }

        IEnumerable<string> ExcludedItems { get; }
    }
}
