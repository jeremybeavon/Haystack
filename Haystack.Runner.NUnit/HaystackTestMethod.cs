using NUnit.Core;

namespace Haystack.Runner.NUnit
{
    public class HaystackTestMethod : NUnitTestMethod
    {
        private readonly HaystackAddin addin;
        private readonly Test test;

        public HaystackTestMethod(HaystackAddin addin, NUnitTestMethod test) : base(test.Method)
        {
            this.addin = addin;
            this.test = test;
        }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            addin.MethodCallTraceManager.SaveFixtureSetUpCallTrace();
            test.Parent = Parent;
            TestResult testResult = test.Run(listener, filter);
            addin.MethodCallTraceManager.SaveCallTrace(test.TestName.FullName);
            return testResult;
        }
    }
}
