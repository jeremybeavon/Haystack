namespace Haystack.Diagnostics.CodeCoverage
{
    public interface IAfterTestRun
    {
        void AfterTestRun(ICodeCoverageContext codeCoverageContext);
    }
}
