namespace Haystack.Diagnostics.CodeCoverage
{
    public interface IBeforeTestRun
    {
        void BeforeTestRun(ITestRunContext testRunContext, ICodeCoverageContext codeCoverageContext);
    }
}
