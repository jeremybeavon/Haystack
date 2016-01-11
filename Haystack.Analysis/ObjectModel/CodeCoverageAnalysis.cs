namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageAnalysis
    {
        public CodeCoverageFile PassingCoverageFile { get; set; }

        public CodeCoverageFile FailingCoverageFile { get; set; }
    }
}
