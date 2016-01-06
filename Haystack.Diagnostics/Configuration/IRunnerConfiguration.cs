using Haystack.Diagnostics.TestIntegration;
using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IRunnerConfiguration
    {
        string RunnerFramework { get; }

        string RunnerFrameworkVersion { get; }

        string RunnerExe { get; }

        IEnumerable<IInitializeTestFramework> InitializeTestFramework { get; }

        IEnumerable<IInitializeTestSuite> InitializeTestSuite { get; }

        IEnumerable<ICleanUpTestSuite> CleanUpTestSuite { get; }

        IEnumerable<IInitializeTestMethod> InitializeTestMethod { get; }

        IEnumerable<ICleanUpTestMethod> CleanUpTestMethod { get; }
    }
}
