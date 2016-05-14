using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Analyzer.Tests.Interception
{
    [TestClass]
    public sealed class NinjectTests
    {
        [TestMethod]
        public void TestSimple()
        {
            HaystackAnalyzerTestRunner.RunHaystackAnalyzer(@"Interception\Ninject\3.2.0\Simple");
        }
    }
}
