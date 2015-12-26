namespace Haystack.Diagnostics.Configuration
{
    public interface ISourceControlConfiguration
    {
        string SourceControlFramework { get; }

        string SourceControlFrameworkVersion { get; }

        string Url { get; }
    }
}
