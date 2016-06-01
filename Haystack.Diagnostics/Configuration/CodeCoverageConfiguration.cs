using System.ComponentModel.DataAnnotations;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class CodeCoverageConfiguration : ICodeCoverageConfiguration
    {
        [Required]
        public string CodeCoverageFramework { get; set; }

        [Required]
        public string CodeCoverageProviderAssembly { get; set; }

        public string CodeCoverageFilter { get; set; }
    }
}
