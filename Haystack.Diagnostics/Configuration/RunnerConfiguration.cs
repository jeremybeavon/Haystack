using Haystack.Core;
using Haystack.Diagnostics.TestIntegration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class RunnerConfiguration : IRunnerConfiguration
    {
        public RunnerConfiguration()
        {
            RunnerInitializers = new List<string>();
            InitializeTestFramework = new List<string>();
            InitializeTestSuite = new List<string>();
            CleanUpTestSuite = new List<string>();
            InitializeTestMethod = new List<string>();
            CleanUpTestMethod = new List<string>();
        }

        [Required]
        public string RunnerFramework { get; set; }

        [Required]
        public string RunnerFrameworkVersion { get; set; }

        [Required]
        public string RunnerExe { get; set; }

        public string AssemblyToTest { get; set; }

        public string RunnerArguments { get; set; }

        public string RunnerArgumentsProvider { get; set; }

        [XmlArrayItem("Type")]
        public List<string> RunnerInitializers { get; set; }

        public string HaystackAddinName { get; set; }

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

        IRunnerArgumentsProvider IRunnerConfiguration.RunnerArgumentsProvider
        {
            get { return TypeResolver.CreateInstance<IRunnerArgumentsProvider>(RunnerArgumentsProvider); }
        }

        IEnumerable<IRunnerInitializer> IRunnerConfiguration.RunnerInitializers
        {
            get { return TypeResolver.CreateInstances<IRunnerInitializer>(RunnerInitializers); }
        }

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
