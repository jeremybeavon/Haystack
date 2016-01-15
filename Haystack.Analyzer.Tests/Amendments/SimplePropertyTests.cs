using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Haystack.Analyzer.Tests.Amendments
{
    [TestClass]
    public class SimplePropertyTests
    {
        [TestMethod]
        public void TestSimpleProperty()
        {
            HaystackAnalyzerTestRunner.RunHaystackAnalyzer(
                @"Examples\Amendments\SimpleProperty",
                @"Passing\haystack.config.xml",
                @"Failing\haystack.config.xml");
        }
    }
}
