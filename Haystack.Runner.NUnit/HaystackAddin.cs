using Haystack.Core;
using Haystack.Diagnostics;
using Haystack.Diagnostics.TestIntegration;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using HaystackBootstrapInitializer = Haystack.Bootstrap.HaystackBootstrapInitializer;

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
            try
            {
                Trace.WriteLine("Initializing Haystack NUnit Addin");
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string configurationFile = Path.Combine(baseDirectory, HaystackConfigurationFile.ConfigurationFileName);
                if (!File.Exists(configurationFile))
                {
                    Trace.WriteLine(string.Format("Skipping Haystack NUnit Addin because configuration file was not found: {0}", configurationFile));
                    return true;
                }

                InitializeHaystack(baseDirectory, configurationFile);
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
            catch (Exception exception)
            {
                Trace.WriteLine(string.Format("Could not initialize Haystack NUnit Addin because of an exception: {0}", exception));
                return false;
            }
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

        private static void InitializeHaystack(string baseDirectory, string configurationFile)
        {
            string bootstrapFile = Path.Combine(baseDirectory, "Haystack.Bootstrap.dll");
            if (File.Exists(bootstrapFile))
            {
                InitializeHaystackUsingBootstrap();
            }
            else
            {
                string haystackDiagnosticsDirectory = BootstrapConfiguration.GetHaystackBaseDirectory(configurationFile);
                AppDomain.CurrentDomain.AddAssemblyResolveDirectory(haystackDiagnosticsDirectory);
                InitializeHaystackUsingDiagnostics(configurationFile);
            }
        }

        private static void InitializeHaystackUsingBootstrap()
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
        }

        private static void InitializeHaystackUsingDiagnostics(string configurationFile)
        {
            HaystackInitializer.InitializeIfNecessary(configurationFile);
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
