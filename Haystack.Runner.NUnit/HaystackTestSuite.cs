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
            addin.MethodCallTraceManager.Initialize();
            TestResult result = base.Run(listener, filter);
            addin.MethodCallTraceManager.SaveFixtureTearDownCallTrace();
            return result;
        }
    }
}
