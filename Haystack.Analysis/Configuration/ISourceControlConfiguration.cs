namespace Haystack.Analysis.Configuration
{
    public interface ISourceControlConfiguration
    {
        string SourceControlFramework { get; }

        string SourceControlProviderAssembly { get; }

        string Url { get; }
    }
}
