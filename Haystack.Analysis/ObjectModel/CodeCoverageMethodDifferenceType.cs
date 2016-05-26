namespace Haystack.Analysis.ObjectModel
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
