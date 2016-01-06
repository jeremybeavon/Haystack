namespace Haystack.Diagnostics.Configuration
{
    public sealed class SourceControlConfiguration : ISourceControlConfiguration
    {
        public string SourceControlFramework { get; set; }

        public string SourceControlProviderAssembly { get; set; }

        public string Url { get; set; }
    }
}
