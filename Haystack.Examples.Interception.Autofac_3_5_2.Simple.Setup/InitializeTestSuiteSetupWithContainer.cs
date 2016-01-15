using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class InitializeTestSuiteSetupWithContainer : IInitializeTestSuite
    {
        public void InitializeTestSuite(string testSuiteName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
