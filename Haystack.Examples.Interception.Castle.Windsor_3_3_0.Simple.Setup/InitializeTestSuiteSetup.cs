using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Castle.Windsor;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple.Setup
{
    public sealed class InitializeTestSuiteSetup : IInitializeTestSuite
    {
        public void InitializeTestSuite(string testSuiteName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
