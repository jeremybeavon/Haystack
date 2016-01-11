using Haystack.Diagnostics.TestIntegration;
using Haystack.Interception.StructureMap;

namespace Haystack.Examples.Interception.StructureMap.Simple.Setup
{
    public sealed class InitializeTestSuiteSetup : IInitializeTestSuite
    {
        public void InitializeTestSuite(string testSuiteName)
        {
            HaystackInterceptionPolicy.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
