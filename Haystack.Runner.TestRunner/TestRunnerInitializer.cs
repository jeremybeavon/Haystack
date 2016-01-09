using Haystack.Diagnostics;
using System.IO;

namespace Haystack.Runner.TestRunner
{
    public sealed class TestRunnerInitializer<TTestIntegrationImplementation> : IRunnerInitializer
    {
        public void InitializeRunner(ITestRunContext testRunContext)
        {
            testRunContext.StandardOutputFile = TestOutput.GetOutputFile<TTestIntegrationImplementation>();
        }
    }
}
