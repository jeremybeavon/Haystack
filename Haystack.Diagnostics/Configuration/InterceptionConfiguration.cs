using System.ComponentModel.DataAnnotations;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class InterceptionConfiguration : IInterceptionConfiguration
    {
        [Required]
        public string InterceptionFramework { get; set; }

        [Required]
        public string InterceptionFrameworkVersion { get; set; }
    }
}
