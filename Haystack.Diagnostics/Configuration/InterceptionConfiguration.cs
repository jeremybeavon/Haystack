namespace Haystack.Diagnostics.Configuration
{
    public sealed class InterceptionConfiguration : IInterceptionConfiguration
    {
        public string InterceptionFramework { get; set; }

        public string InterceptionFrameworkVersion { get; set; }
    }
}
