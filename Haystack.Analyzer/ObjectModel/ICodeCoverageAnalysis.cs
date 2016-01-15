namespace Haystack.Analyzer.ObjectModel
{
    public interface ICodeCoverageAnalysis
    {
        ICodeCoverageFile PassingCoverageFile { get; }

        ICodeCoverageFile FailingCoverageFile { get; }
    }
}
