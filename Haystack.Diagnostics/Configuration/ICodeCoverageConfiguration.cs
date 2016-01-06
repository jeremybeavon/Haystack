namespace Haystack.Diagnostics.Configuration
{
    public interface ICodeCoverageConfiguration
    {
        string CodeCoverageFramework { get; }

        string CodeCoverageProviderAssembly { get; }

        string CodeCoverageFilter { get; }
    }
}
