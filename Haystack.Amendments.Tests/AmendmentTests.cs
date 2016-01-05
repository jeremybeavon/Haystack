using FluentAssertions;
using Haystack.Amendments.Tests.Amendments;
using Haystack.Amendments.Tests.StrongNamedTestRunner;
using Haystack.Amendments.Tests.TestRunner;
using Haystack.Diagnostics.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Haystack.Amendments.Tests
{
    [TestClass]
    public sealed class AmendmentTests
    {
        [TestMethod]
        public void TestBeforePropertyGetAmendment()
        {
            const string expectedResult = "BeforePropertyGet(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleProperty, " +
                "propertyName = TestProperty)";
            (new AmendmentTestRunner()
            {
                TestName = "BeforePropertyGet",
                TestRunnerMethod = DefaultTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforePropertyGetAmendments = new List<string>()
                        {
                            typeof(BeforePropertyGetAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforeStrongNamedPropertyGetAmendment()
        {
            const string expectedResult = "BeforePropertyGet(" +
                "instance = Haystack.Amendments.Tests.StrongNamedTestTarget.SimpleProperty, " +
                "propertyName = TestProperty)";
            (new StrongNamedAmendmentTestRunner()
            {
                TestName = "BeforeStrongNamedPropertyGet",
                TestRunnerMethod = DefaultStrongNamedTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforePropertyGetAmendments = new List<string>()
                        {
                            typeof(BeforePropertyGetAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }
    }
}
