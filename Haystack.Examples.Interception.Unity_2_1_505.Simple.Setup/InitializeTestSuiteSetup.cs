using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Unity;

namespace Haystack.Examples.Interception.Unity.Simple.Setup
{
    public sealed class InitializeTestSuiteSetup : IInitializeTestSuite
    {
        public void InitializeTestSuite(string testSuiteName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
