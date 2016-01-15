using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Ninject;

namespace Haystack.Examples.Interception.Ninject.Simple.Setup
{
    public sealed class InitializeTestSuiteSetup : IInitializeTestSuite
    {
        public void InitializeTestSuite(string testSuiteName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleKernel);
        }
    }
}
