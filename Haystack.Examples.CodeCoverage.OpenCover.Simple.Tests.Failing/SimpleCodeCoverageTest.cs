using NUnit.Framework;

namespace Haystack.Examples.CodeCoverage.OpenCover.Simple.Tests
{
    [TestFixture]
    public sealed class SimpleCodeCoverageTest
    {
        [Test]
        public void TestMethodCalls()
        {
            TestMethod1();
            TestMethod2();
            TestMethod3();
        }

        public void TestMethod1()
        {
        }

        public void TestMethod2()
        {
        }

        public void TestMethod3()
        {
        }
    }
}
