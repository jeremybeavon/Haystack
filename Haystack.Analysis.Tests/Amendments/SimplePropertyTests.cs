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
            const string baseDirectory = @"Examples\Amendments\SimpleProperty";
            HaystackAnalysisTestRunner.RunHaystackAnalysis(
                Path.Combine(baseDirectory, @"Passing\haystack.config.xml"),
                Path.Combine(baseDirectory, @"Failing\haystack.config.xml"));
        }
    }
}
