using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Ninject;

namespace Haystack.Examples.Interception.Ninject.Simple.Setup
{
    public sealed class InitializeTestFrameworkSetup : IInitializeTestFramework
    {
        public void InitializeTestFramework()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleKernel);
        }
    }
}
