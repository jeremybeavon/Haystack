using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Analyzer.Tests.Amendments
{
    [TestClass]
    public class SimplePropertyTests
    {
        [TestMethod]
        public void TestSimpleProperty()
        {
            HaystackAnalyzerTestRunner.RunHaystackAnalyzer(@"Amendments\SimpleProperty");
        }
    }
}
