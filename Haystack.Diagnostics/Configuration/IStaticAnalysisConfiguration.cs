using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IStaticAnalysisConfiguration
    {
        string StaticAnalysisFramework { get; }

        IEnumerable<string> Paths { get; }
    }
}
