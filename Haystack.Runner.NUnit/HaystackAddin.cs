using Haystack.Diagnostics.TestIntegration;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Haystack.Runner.NUnit
{
    [NUnitAddin]
    public sealed class HaystackAddin : IAddin, ISuiteBuilder, ITestCaseBuilder2
    {
        private SuiteBuilderCollection builders;
        private NUnitTestCaseBuilder testCaseBuilder;
        private MethodCallTraceManager methodCallTraceManager;

        public HaystackAddin()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public MethodCallTraceManager MethodCallTraceManager
        {
            get { return methodCallTraceManager ?? (methodCallTraceManager = new MethodCallTraceManager()); }
        }

        public bool Install(IExtensionHost host)
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Haystack.Diagnostics.dll")))
                return true;

            InitializeTestFramework();
            builders = new SuiteBuilderCollection(host);
            builders.Install(new NUnitTestFixtureBuilder());
            builders.Install(new SetUpFixtureBuilder());
            testCaseBuilder = new NUnitTestCaseBuilder();
            IExtensionPoint suiteBuilders = host.GetExtensionPoint("SuiteBuilders");
            suiteBuilders.Install(this);
            IExtensionPoint testCaseBuilders = host.GetExtensionPoint("TestCaseBuilders");
            testCaseBuilders.Install(this);
            return true;
        }

        public bool CanBuildFrom(Type type)
        {
            return builders.CanBuildFrom(type);
        }

        public Test BuildFrom(Type type)
        {
            Test test = builders.BuildFrom(type);
            NUnitTestFixture fixture = test as NUnitTestFixture;
            return fixture == null ? test : new HaystackTestSuite(this, fixture);
        }

        public bool CanBuildFrom(MethodInfo method)
        {
            return testCaseBuilder.CanBuildFrom(method);
        }

        public Test BuildFrom(MethodInfo method)
        {
            return DecorateTest(testCaseBuilder.BuildFrom(method));
        }

        public bool CanBuildFrom(MethodInfo method, Test suite)
        {
            return testCaseBuilder.CanBuildFrom(method, suite);
        }

        public Test BuildFrom(MethodInfo method, Test suite)
        {
            return DecorateTest(testCaseBuilder.BuildFrom(method, suite));
        }

        public static void InitializeOrCleanUp<T>(IEnumerable<T> interfaces, Action<T> action)
        {
            if (interfaces != null)
            {
                foreach (T @interface in interfaces)
                {
                    action(@interface);
                }
            }
        }

        private static void InitializeTestFramework()
        {
            InitializeOrCleanUp(
                TestIntegrationRepository.IntitializeTestFrameworkMethods,
                initialize => initialize.InitializeTestFramework());
        }

        private Test DecorateTest(Test test)
        {
            if (test.Tests == null)
                return new HaystackTestMethod(this, (NUnitTestMethod)test);

            for (int index = 0; index < test.Tests.Count; index++)
            {
                test.Tests[index] = new HaystackTestMethod(this, (NUnitTestMethod)test.Tests[index]);
            }

            return test;
        }
    }
}
