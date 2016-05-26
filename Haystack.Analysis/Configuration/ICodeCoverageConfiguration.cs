namespace Haystack.Analysis.Configuration
{
    public interface ICodeCoverageConfiguration
    {
        string CodeCoverageFramework { get; }

        string CodeCoverageAnalysisProviderAssembly { get; }
    }
}
