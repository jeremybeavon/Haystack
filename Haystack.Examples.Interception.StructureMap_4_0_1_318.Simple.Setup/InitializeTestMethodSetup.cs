using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.StructureMap;

namespace Haystack.Examples.Interception.StructureMap.Simple.Setup
{
    public sealed class InitializeTestMethodSetup : IInitializeTestMethod
    {
        public void InitializeTestMethod(string testMethodName)
        {
            HaystackInterceptionPolicy.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
