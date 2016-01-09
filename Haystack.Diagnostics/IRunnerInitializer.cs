namespace Haystack.Diagnostics
{
    public interface IRunnerInitializer
    {
        void InitializeRunner(ITestRunContext testRunContext);
    }
}
