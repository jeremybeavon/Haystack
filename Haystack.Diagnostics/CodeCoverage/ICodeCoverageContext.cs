namespace Haystack.Diagnostics.CodeCoverage
{
    public interface ICodeCoverageContext
    {
        string CodeCoverageFilter { get; }

        string OutputDirectory { get; }
    }
}
