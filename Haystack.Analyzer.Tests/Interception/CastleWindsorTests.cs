using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Analyzer.Tests.Interception
{
    [TestClass]
    public sealed class CastleWindsorTests
    {
        [TestMethod]
        public void TestSimple()
        {
            HaystackAnalyzerTestRunner.RunHaystackAnalyzer(@"Interception\Castle.Windsor\3.3.0\Simple");
        }
    }
}
