using FluentAssertions;
using Haystack.Core;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using Haystack.Diagnostics.TestIntegration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Haystack.Runner.TestRunner
{
    public abstract class AbstractTestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation>
        where TTestIntegrationImplementation : TTestIntegrationInterface
    {
        protected virtual string AssemblyToTest
        {
            get { return GetType().Assembly.AssemblyFilePath(); }
        }

        protected abstract string HaystackBaseDirectory { get; }

        protected abstract string RunnerFramework { get; }

        protected abstract string RunnerFrameworkVersion { get; }

        protected abstract string RunnerExe { get; }

        private string OutputDirectory
        {
            get { return TestOutput.OutputDirectory; }
        }

        public void Execute()
        {
            HaystackConfiguration configuration = CreateConfiguration();
            GetList(configuration.Runner).Add(new TypeConfiguration(typeof(TTestIntegrationImplementation)));
            configuration.Initialize();
            string path = Path.Combine(Path.GetDirectoryName(AssemblyToTest), HaystackConfigurationFile.ConfigurationFileName);
            File.WriteAllText(path, configuration.ToString());
            HaystackRunner.RunHaystackDiagnostics(configuration);
            string expectedText = typeof(TTestIntegrationImplementation).GetCustomAttribute<DescriptionAttribute>().Description;
            string actualText = File.ReadAllText(TestOutput.GetOutputFile<TTestIntegrationImplementation>());
            Trace.WriteLine(actualText);
            actualText.Should().Contain(expectedText);
        }

        private HaystackConfiguration CreateConfiguration()
        {
            return new HaystackConfiguration()
            {
                OutputDirectory = OutputDirectory,
                HaystackBaseDirectory = HaystackBaseDirectory,
                Runner = new RunnerConfiguration()
                {
                    RunnerFramework = RunnerFramework,
                    RunnerFrameworkVersion = RunnerFrameworkVersion,
                    RunnerExe = RunnerExe,
                    RunnerArguments = string.Format("\"{0}\"", AssemblyToTest),
                    AssemblyToTest = AssemblyToTest,
                    RunnerInitializers = new List<TypeConfiguration>()
                    {
                        new TypeConfiguration(typeof(TestRunnerInitializer<TTestIntegrationImplementation>))
                    }
                }
            };
        }

        private List<TypeConfiguration> GetList(RunnerConfiguration configuration)
        {
            if (typeof(TTestIntegrationInterface) == typeof(IInitializeTestFramework))
            {
                return configuration.InitializeTestFramework;
            }

            if (typeof(TTestIntegrationInterface) == typeof(IInitializeTestSuite))
            {
                return configuration.InitializeTestSuite;
            }

            if (typeof(TTestIntegrationInterface) == typeof(ICleanUpTestSuite))
            {
                return configuration.CleanUpTestSuite;
            }

            if (typeof(TTestIntegrationInterface) == typeof(IInitializeTestMethod))
            {
                return configuration.InitializeTestMethod;
            }

            if (typeof(TTestIntegrationInterface) == typeof(ICleanUpTestMethod))
            {
                return configuration.CleanUpTestMethod;
            }

            throw new InvalidOperationException(string.Format("Unknown TestIntegration interface: {0}", typeof(TTestIntegrationInterface).FullName));
        }
    }
}
