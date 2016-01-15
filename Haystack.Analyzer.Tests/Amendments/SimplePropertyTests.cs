using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Haystack.Analysis.Tests.Amendments
{
    [TestClass]
    public class SimplePropertyTests
    {
        [TestMethod]
        public void TestSimpleProperty()
        {
            HaystackAnalysisTestRunner.RunHaystackAnalysis(
                @"Examples\Amendments\SimpleProperty",
                @"Passing\haystack.config.xml",
                @"Failing\haystack.config.xml");
        }
    }
}
