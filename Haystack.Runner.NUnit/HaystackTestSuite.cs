using Haystack.Diagnostics.TestIntegration;
using NUnit.Core;

namespace Haystack.Runner.NUnit
{
    public class HaystackTestSuite : NUnitTestFixture
    {
        private readonly HaystackAddin addin;

        public HaystackTestSuite(HaystackAddin addin, NUnitTestFixture test) : base(test.FixtureType)
        {
            this.addin = addin;
            Add(test);
        }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            HaystackAddin.InitializeOrCleanUp(
                TestIntegrationRepository.IntitializeTestSuiteMethods,
                suite => suite.InitializeTestSuite(TestName.FullName));
            TestResult result = base.Run(listener, filter);
            addin.MethodCallTraceManager.SaveFixtureTearDownCallTrace();
            HaystackAddin.InitializeOrCleanUp(
                TestIntegrationRepository.CleanUpTestSuiteMethods,
                suite => suite.CleanUpTestSuite(TestName.FullName));
            return result;
        }
    }
}
