using Haystack.Bootstrap;
using Haystack.Diagnostics.TestIntegration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Haystack.Runner.NUnit
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class HaystackDiagnosticsAttribute : TestActionAttribute
    {
        private MethodCallTraceManager methodCallTraceManager;

        public HaystackDiagnosticsAttribute()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            try
            {
                Initialize();
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Could not initialize HaystackDiagnostics: " + exception);
            }
        }

        private MethodCallTraceManager MethodCallTraceManager
        {
            get { return methodCallTraceManager ?? (methodCallTraceManager = new MethodCallTraceManager()); }
        }

        public override ActionTargets Targets
        {
            get { return ActionTargets.Suite | ActionTargets.Test; }
        }

        public override void BeforeTest(ITest test)
        {
            if (test.IsSuite)
            {
                InitializeOrCleanUp(
                    TestIntegrationRepository.IntitializeTestSuiteMethods,
                    suite => suite.InitializeTestSuite(test.FullName));
            }
            else
            {
                InitializeOrCleanUp(
                    TestIntegrationRepository.InitializeTestMethodMethods,
                    method => method.InitializeTestMethod(test.FullName));
                MethodCallTraceManager.SaveFixtureSetUpCallTrace();
            }
        }

        public override void AfterTest(ITest test)
        {
            if (test.IsSuite)
            {
                MethodCallTraceManager.SaveFixtureTearDownCallTrace();
                InitializeOrCleanUp(
                    TestIntegrationRepository.CleanUpTestSuiteMethods,
                    suite => suite.CleanUpTestSuite(test.FullName));
            }
            else
            {
                MethodCallTraceManager.SaveCallTrace(test.FullName);
                InitializeOrCleanUp(
                    TestIntegrationRepository.CleanUpTestMethodMethods,
                    method => method.CleanUpTestMethod(test.FullName));
            }
        }

        private static void Initialize()
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            InitializeTestFramework();
        }

        private static void InitializeTestFramework()
        {
            InitializeOrCleanUp(
                TestIntegrationRepository.IntitializeTestFrameworkMethods,
                framework => framework.InitializeTestFramework());
        }

        private static void InitializeOrCleanUp<T>(IEnumerable<T> interfaces, Action<T> action)
        {
            if (interfaces != null)
            {
                foreach (T @interface in interfaces)
                {
                    action(@interface);
                }
            }
        }
    }
}
