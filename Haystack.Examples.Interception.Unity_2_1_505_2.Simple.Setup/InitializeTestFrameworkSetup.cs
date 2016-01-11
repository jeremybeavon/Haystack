using Haystack.Diagnostics.TestIntegration;
using Haystack.Interception.Unity;

namespace Haystack.Examples.Interception.Unity.Simple.Setup
{
    public sealed class InitializeTestFrameworkSetup : IInitializeTestFramework
    {
        public void InitializeTestFramework()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
