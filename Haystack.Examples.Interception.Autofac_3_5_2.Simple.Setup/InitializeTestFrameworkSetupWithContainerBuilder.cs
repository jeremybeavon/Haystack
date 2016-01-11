using Haystack.Diagnostics.TestIntegration;
using Haystack.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class InitializeTestFrameworkSetupWithContainerBuilder : IInitializeTestFramework
    {
        public void InitializeTestFramework()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainerBuilder);
        }
    }
}
