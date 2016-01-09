using FluentAssertions;
using Haystack.Diagnostics.TestIntegration;
using Haystack.Runner.NUnit.Tests.TestIntegrations;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Runner.NUnit.Tests
{
    [TestClass]
    [TestFixture]
    public class HaystackNUnit_2_6_4_AddinTests
    {
        [TestMethod]
        public void TestInitializeTestFrameworkForNUnit_2_6_4()
        {
            new TestIntegrationRunner<IInitializeTestFramework, InitializeTestFrameworkIntegration>().Execute();
        }

        [TestMethod]
        public void TestInitializeTestSuiteForNUnit_2_6_4()
        {
            new TestIntegrationRunner<IInitializeTestSuite, InitializeTestSuiteIntegration>().Execute();
        }

        [TestMethod]
        public void TestCleanUpTestSuiteForNUnit_2_6_4()
        {
            new TestIntegrationRunner<ICleanUpTestSuite, CleanUpTestSuiteIntegration>().Execute();
        }

        [TestMethod]
        public void TestInitializeTestMethodForNUnit_2_6_4()
        {
            new TestIntegrationRunner<IInitializeTestMethod, InitializeTestMethodIntegration>().Execute();
        }

        [TestMethod]
        public void TestCleanUpTestMethodForNUnit_2_6_4()
        {
            new TestIntegrationRunner<ICleanUpTestMethod, CleanUpTestMethodIntegration>().Execute();
        }

        [Test]
        public void TestAddition()
        {
            (1 + 1).Should().Be(2);
        }
    }
}
