using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Castle.Windsor;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple.Setup
{
    public sealed class InitializeTestFrameworkSetup : IInitializeTestFramework
    {
        public void InitializeTestFramework()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
