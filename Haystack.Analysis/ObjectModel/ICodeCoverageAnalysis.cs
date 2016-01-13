namespace Haystack.Analysis.ObjectModel
{
    public interface ICodeCoverageAnalysis
    {
        ICodeCoverageFile PassingCoverageFile { get; }

        ICodeCoverageFile FailingCoverageFile { get; }
    }
}
