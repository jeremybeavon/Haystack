using Haystack.Diagnostics.TestIntegration;
using Haystack.Interception.Castle.Windsor;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple.Setup
{
    public sealed class InitializeTestMethodSetup : IInitializeTestMethod
    {
        public void InitializeTestMethod(string testMethodName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
