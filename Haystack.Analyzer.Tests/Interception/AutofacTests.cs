using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Analyzer.Tests.Interception
{
    [TestClass]
    public sealed class AutofacTests
    {
        [TestMethod]
        public void TestSimple()
        {
            HaystackAnalyzerTestRunner.RunHaystackAnalyzer(@"Interception\Autofac\3.5.2\Simple");
        }
    }
}
