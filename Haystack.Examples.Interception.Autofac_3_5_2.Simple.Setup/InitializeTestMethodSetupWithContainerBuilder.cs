using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class InitializeTestMethodSetupWithContainerBuilder : IInitializeTestMethod
    {
        public void InitializeTestMethod(string testMethodName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainerBuilder);
        }
    }
}
