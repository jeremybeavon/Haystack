using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface ICodeCoverageConfiguration
    {
        string CodeCoverageFramework { get; }

        string CodeCoverageFrameworkVersion { get; }

        IEnumerable<string> IncludedItems { get; }

        IEnumerable<string> ExcludedItems { get; }
    }
}
