using System;

namespace Haystack.Analysis.Configuration
{
    public sealed class CodeCoverageConfiguration : ICodeCoverageConfiguration
    {
        public string CodeCoverageFramework { get; set; }

        public string CodeCoverageAnalysisProviderAssembly { get; set; }
    }
}
