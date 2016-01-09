using Haystack.Diagnostics.Configuration;
using System.Collections.Generic;

namespace Haystack.Diagnostics.TestIntegration
{
    public static class TestIntegrationRepository
    {
        public static IEnumerable<IInitializeTestFramework> IntitializeTestFrameworkMethods { get; set; }

        public static IEnumerable<IInitializeTestSuite> IntitializeTestSuiteMethods { get; set; }

        public static IEnumerable<ICleanUpTestSuite> CleanUpTestSuiteMethods { get; set; }

        public static IEnumerable<IInitializeTestMethod> InitializeTestMethodMethods { get; set; }

        public static IEnumerable<ICleanUpTestMethod> CleanUpTestMethodMethods { get; set; }

        public static void Initialize(IRunnerConfiguration configuration)
        {
            IntitializeTestFrameworkMethods = configuration.InitializeTestFramework;
            IntitializeTestSuiteMethods = configuration.InitializeTestSuite;
            CleanUpTestSuiteMethods = configuration.CleanUpTestSuite;
            InitializeTestMethodMethods = configuration.InitializeTestMethod;
            CleanUpTestMethodMethods = configuration.CleanUpTestMethod;
        }
    }
}
