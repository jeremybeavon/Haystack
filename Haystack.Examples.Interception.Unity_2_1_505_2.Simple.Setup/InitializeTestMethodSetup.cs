using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Unity;

namespace Haystack.Examples.Interception.Unity.Simple.Setup
{
    public sealed class InitializeTestMethodSetup : IInitializeTestMethod
    {
        public void InitializeTestMethod(string testMethodName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
