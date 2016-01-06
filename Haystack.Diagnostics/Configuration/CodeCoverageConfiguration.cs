namespace Haystack.Diagnostics.Configuration
{
    public sealed class CodeCoverageConfiguration : ICodeCoverageConfiguration
    {
        public string CodeCoverageFramework { get; set; }

        public string CodeCoverageProviderAssembly { get; set; }

        public string CodeCoverageFilter { get; set; }
    }
}
