namespace Haystack.Diagnostics.Configuration
{
    public interface ICodeCoverageConfiguration
    {
        string CodeCoverageFramework { get; }

        string CodeCoverageFilter { get; }
    }
}
