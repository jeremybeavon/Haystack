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
        public void TestAfterPropertyGetAmendment()
        {
            const string expectedResult = "AfterPropertyGet(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleProperty, " +
                "propertyName = TestProperty, " +
                "value = Instance2)";
            (new AmendmentTestRunner()
            {
                TestName = "AfterPropertyGet",
                TestRunnerMethod = DefaultTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        AfterPropertyGetAmendments = new List<string>()
                        {
                            typeof(AfterPropertyGetAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforePropertySetAmendment()
        {
            const string expectedResult = "BeforePropertySet(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleProperty, " +
                "propertyName = TestProperty, " +
                "value = Instance2)";
            (new AmendmentTestRunner()
            {
                TestName = "BeforePropertySet",
                TestRunnerMethod = DefaultTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforePropertySetAmendments = new List<string>()
                        {
                            typeof(BeforePropertySetAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterPropertySetAmendment()
        {
            const string expectedResult = "AfterPropertySet(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleProperty, " +
                "propertyName = TestProperty, " +
                "value = Instance2)";
            (new AmendmentTestRunner()
            {
                TestName = "AfterPropertySet",
                TestRunnerMethod = DefaultTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        AfterPropertySetAmendments = new List<string>()
                        {
                            typeof(AfterPropertySetAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforeConstructorAmendment()
        {
            const string expectedResult = "BeforeConstructor(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleProperty, " +
                "parameters = Instance1)";
            (new AmendmentTestRunner()
            {
                TestName = "BeforeConstructor",
                TestRunnerMethod = DefaultTestRunner.ConstructorTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforeConstructorAmendments = new List<string>()
                        {
                            typeof(BeforeConstructorAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterConstructorAmendment()
        {
            const string expectedResult = "AfterConstructor(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleProperty, " +
                "parameters = Instance1)";
            (new AmendmentTestRunner()
            {
                TestName = "AfterConstructor",
                TestRunnerMethod = DefaultTestRunner.ConstructorTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        AfterConstructorAmendments = new List<string>()
                        {
                            typeof(AfterConstructorAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforeMethodAmendment()
        {
            const string expectedResult = "BeforeMethod(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleVoidMethod, " +
                "methodName = TestMethod, " +
                "parameters = )";
            (new AmendmentTestRunner()
            {
                TestName = "BeforeMethod",
                TestRunnerMethod = DefaultTestRunner.VoidMethodTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforeMethodAmendments = new List<string>()
                        {
                            typeof(BeforeMethodAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterVoidMethodAmendment()
        {
            const string expectedResult = "AfterVoidMethod(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleVoidMethod, " +
                "methodName = TestMethod, " +
                "parameters = )";
            (new AmendmentTestRunner()
            {
                TestName = "AfterVoidMethod",
                TestRunnerMethod = DefaultTestRunner.VoidMethodTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        AfterVoidMethodAmendments = new List<string>()
                        {
                            typeof(AfterVoidMethodAmendment).AssemblyQualifiedName
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        [Ignore]
        public void TestAfterMethodAmendment()
        {
            const string expectedResult = "AfterMethod(" +
                "instance = Haystack.Amendments.Tests.TestTarget.SimpleVoidMethod, " +
                "methodName = TestMethod, " +
                "parameters = , " +
                "returnValue = 1)";
            (new AmendmentTestRunner()
            {
                TestName = "AfterMethod",
                TestRunnerMethod = DefaultTestRunner.MethodTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        AfterMethodAmendments = new List<string>()
                        {
                            typeof(AfterMethodAmendment).AssemblyQualifiedName
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
