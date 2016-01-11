using Haystack.Diagnostics.TestIntegration;
using Haystack.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class InitializeTestFrameworkSetupWithContainer : IInitializeTestFramework
    {
        public void InitializeTestFramework()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
