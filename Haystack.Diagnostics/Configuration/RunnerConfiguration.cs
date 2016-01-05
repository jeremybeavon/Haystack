using Haystack.Diagnostics.TestIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class RunnerConfiguration : IRunnerConfiguration
    {
        public string RunnerFramework { get; set; }

        public string RunnerFrameworkVersion { get; set; }

        public List<string> InitializeTestFramework { get; set; }

        public List<string> InitializeTestSuite { get; set; }

        public List<string> CleanUpTestSuite { get; set; }

        public List<string> InitializeTestMethod { get; set; }

        public List<string> CleanUpTestMethod { get; set; }

        IEnumerable<IInitializeTestFramework> IRunnerConfiguration.InitializeTestFramework
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IEnumerable<IInitializeTestSuite> IRunnerConfiguration.InitializeTestSuite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IEnumerable<ICleanUpTestSuite> IRunnerConfiguration.CleanUpTestSuite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IEnumerable<IInitializeTestMethod> IRunnerConfiguration.InitializeTestMethod
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IEnumerable<ICleanUpTestMethod> IRunnerConfiguration.CleanUpTestMethod
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
