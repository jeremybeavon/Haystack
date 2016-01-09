namespace Haystack.Diagnostics.Configuration
{
    public sealed class CodeCoverageConfiguration : ICodeCoverageConfiguration
    {
        public string CodeCoverageFramework { get; }

        public string CodeCoverageProviderAssembly { get; }

        public string CodeCoverageFilter { get; }
    }
}
