namespace Haystack.Analyzer.ObjectModel
{
    public enum CodeCoverageMethodDifferenceType
    {
        NoData,
        NoCoverage,
        IdenticalCodeCoverage,
        CodeCoverageDifferentAndTextIdentical,
        CodeCoverageDifferentAndTextDifferent,
        PassingCodeCoverageMethodMissing,
        FailingCodeCoverageMethodMissing
    }
}
