using System.ComponentModel.DataAnnotations;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class CodeCoverageConfiguration : ICodeCoverageConfiguration
    {
        [Required]
        public string CodeCoverageFramework { get; }

        [Required]
        public string CodeCoverageProviderAssembly { get; }

        public string CodeCoverageFilter { get; }
    }
}
