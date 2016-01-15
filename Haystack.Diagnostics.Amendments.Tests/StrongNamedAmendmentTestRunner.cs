using System.IO;

namespace Haystack.Amendments.Tests
{
    public sealed class StrongNamedAmendmentTestRunner : AmendmentTestRunner
    {
        public StrongNamedAmendmentTestRunner()
        {
            string testTargetDirectory = Path.Combine(BaseDirectory, "StrongNamedTestTarget");
            StrongNameKey = Path.Combine(testTargetDirectory, "Haystack.snk");
            TargetDll = Path.Combine(testTargetDirectory, "Haystack.Amendments.Tests.StrongNamedTestTarget.dll");
            TestRunnerDll = Path.Combine(testTargetDirectory, "Haystack.Amendments.Tests.StrongNamedTestRunner.dll");
        }
    }
}
