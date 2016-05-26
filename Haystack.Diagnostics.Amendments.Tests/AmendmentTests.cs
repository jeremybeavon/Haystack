using FluentAssertions;
using Haystack.Core;
using Haystack.Diagnostics.Amendments.Tests.Amendments;
using Haystack.Diagnostics.Amendments.Tests.StrongNamedTestRunner;
using Haystack.Diagnostics.Amendments.Tests.TestRunner;
using Haystack.Diagnostics.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Haystack.Diagnostics.Amendments.Tests
{
    [TestClass]
    public sealed class AmendmentTests
    {
        [TestMethod]
        public void TestBeforePropertyGetAmendment()
        {
            const string expectedResult = "BeforePropertyGet(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleProperty, " +
                "propertyName = TestProperty)";
            (new AmendmentTestRunner()
            {
                TestName = "BeforePropertyGet",
                TestRunnerMethod = DefaultTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforePropertyGetAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(BeforePropertyGetAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterPropertyGetAmendment()
        {
            const string expectedResult = "AfterPropertyGet(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleProperty, " +
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
                        AfterPropertyGetAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(AfterPropertyGetAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforePropertySetAmendment()
        {
            const string expectedResult = "BeforePropertySet(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleProperty, " +
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
                        BeforePropertySetAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(BeforePropertySetAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterPropertySetAmendment()
        {
            const string expectedResult = "AfterPropertySet(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleProperty, " +
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
                        AfterPropertySetAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(AfterPropertySetAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforeConstructorAmendment()
        {
            const string expectedResult = "BeforeConstructor(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleProperty, " +
                "parameters = Instance1)";
            (new AmendmentTestRunner()
            {
                TestName = "BeforeConstructor",
                TestRunnerMethod = DefaultTestRunner.ConstructorTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforeConstructorAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(BeforeConstructorAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterConstructorAmendment()
        {
            const string expectedResult = "AfterConstructor(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleProperty, " +
                "parameters = Instance1)";
            (new AmendmentTestRunner()
            {
                TestName = "AfterConstructor",
                TestRunnerMethod = DefaultTestRunner.ConstructorTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        AfterConstructorAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(AfterConstructorAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforeMethodAmendment()
        {
            const string expectedResult = "BeforeMethod(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleVoidMethod, " +
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
                        BeforeMethodAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(BeforeMethodAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestAfterVoidMethodAmendment()
        {
            const string expectedResult = "AfterVoidMethod(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleVoidMethod, " +
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
                        AfterVoidMethodAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(AfterVoidMethodAmendment))
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
                "instance = Haystack.Diagnostics.Amendments.Tests.TestTarget.SimpleVoidMethod, " +
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
                        AfterMethodAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(AfterMethodAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestBeforeStrongNamedPropertyGetAmendment()
        {
            const string expectedResult = "BeforePropertyGet(" +
                "instance = Haystack.Diagnostics.Amendments.Tests.StrongNamedTestTarget.SimpleProperty, " +
                "propertyName = TestProperty)";
            (new StrongNamedAmendmentTestRunner()
            {
                TestName = "BeforeStrongNamedPropertyGet",
                TestRunnerMethod = DefaultStrongNamedTestRunner.PropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        BeforePropertyGetAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(BeforePropertyGetAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }

        [TestMethod]
        public void TestHaystackPropertyAmendment()
        {
            const string expectedResult = "Threads: 1, Calls: 2";
            (new AmendmentTestRunner()
            {
                TestName = "HaystackProperty",
                TestRunnerMethod = DefaultTestRunner.HaystackPropertyTest,
                Configuration = new HaystackConfiguration()
                {
                    Amendments = new AmendmentConfiguration()
                    {
                        HaystackPropertyAmendments = new List<TypeConfiguration>()
                        {
                            new TypeConfiguration(typeof(HaystackPropertyAmendment))
                        }
                    }
                }
            }).RunTest().Should().Be(expectedResult);
        }
    }
}
