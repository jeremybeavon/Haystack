using Haystack.Diagnostics.TestIntegration;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class RunnerConfiguration : IRunnerConfiguration
    {
        public string RunnerFramework { get; set; }

        public string RunnerFrameworkVersion { get; set; }

        public string RunnerExe { get; set; }

        public string RunnerArguments { get; set; }

        [XmlArrayItem("Type")]
        public List<string> InitializeTestFramework { get; set; }

        [XmlArrayItem("Type")]
        public List<string> InitializeTestSuite { get; set; }

        [XmlArrayItem("Type")]
        public List<string> CleanUpTestSuite { get; set; }

        [XmlArrayItem("Type")]
        public List<string> InitializeTestMethod { get; set; }

        [XmlArrayItem("Type")]
        public List<string> CleanUpTestMethod { get; set; }

        IEnumerable<IInitializeTestFramework> IRunnerConfiguration.InitializeTestFramework
        {
            get { return TypeResolver.CreateInstances<IInitializeTestFramework>(InitializeTestFramework); }
        }

        IEnumerable<IInitializeTestSuite> IRunnerConfiguration.InitializeTestSuite
        {
            get { return TypeResolver.CreateInstances<IInitializeTestSuite>(InitializeTestSuite); }
        }

        IEnumerable<ICleanUpTestSuite> IRunnerConfiguration.CleanUpTestSuite
        {
            get { return TypeResolver.CreateInstances<ICleanUpTestSuite>(CleanUpTestSuite); }
        }

        IEnumerable<IInitializeTestMethod> IRunnerConfiguration.InitializeTestMethod
        {
            get { return TypeResolver.CreateInstances<IInitializeTestMethod>(InitializeTestMethod); }
        }

        IEnumerable<ICleanUpTestMethod> IRunnerConfiguration.CleanUpTestMethod
        {
            get { return TypeResolver.CreateInstances<ICleanUpTestMethod>(CleanUpTestMethod); }
        }
    }
}
