using FluentAssertions;
using Haystack.Analysis.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Analyzer.Tests.Interception
{
    [TestClass]
    public sealed class AutofacTests
    {
        [TestMethod]
        public void TestSimpleAutofac3_5_2()
        {
            HaystackAnalysis analysis = HaystackAnalyzerTestRunner.RunHaystackAnalyzer(@"Interception\Autofac\3.5.2\Simple");
            analysis.HaystackMethods.Count.Should().Be(3);
        }
    }
}
