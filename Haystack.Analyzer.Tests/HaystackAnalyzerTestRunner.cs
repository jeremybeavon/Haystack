using Haystack.Core;
using Haystack.Core.IO;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using System;
using System.IO;

namespace Haystack.Analyzer.Tests
{
    public static class HaystackAnalyzerTestRunner
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string haystackBaseDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\Haystack"));
        private static readonly string haystackDiagnosticsDirectory = 
            Path.Combine(haystackBaseDirectory, "Runner", FrameworkVersion.Current, "Diagnostics");

        public static void RunHaystackAnalyzer(string exampleDirectory, string passingConfigurationFile, string failingConfigurationFile)
        {
            string testDirectory = InitializeTestDirectory(exampleDirectory);
            passingConfigurationFile = InitializeConfigurationFile(testDirectory, passingConfigurationFile);
            failingConfigurationFile = InitializeConfigurationFile(testDirectory, failingConfigurationFile);
            RunHaystackRunner(passingConfigurationFile);
            RunHaystackRunner(failingConfigurationFile);
            RunHaystackAnalyzer(passingConfigurationFile, failingConfigurationFile);
        }

        private static string InitializeTestDirectory(string exampleDirectory)
        {
            string originalDirectory = Path.Combine(haystackDiagnosticsDirectory, exampleDirectory);
            string testDirectory = Path.Combine(baseDirectory, exampleDirectory);
            DirectoryCopy.CopyDirectory(originalDirectory, exampleDirectory);
            return testDirectory;
        }

        private static string InitializeConfigurationFile(string testDirectory, string configurationFile)
        {
            configurationFile = Path.Combine(testDirectory, configurationFile);
            HaystackConfiguration haystackConfiguration = HaystackConfiguration.LoadText(File.ReadAllText(configurationFile));
            haystackConfiguration.HaystackDiagnosticsDirectory = haystackDiagnosticsDirectory;
            File.WriteAllText(configurationFile, haystackConfiguration.ToString());
            return configurationFile;
        }

        private static void RunHaystackRunner(string configurationFile)
        {
            RunExecutable(Path.Combine(haystackDiagnosticsDirectory, @"Runner\Haystack.Runner.exe"), "--ConfigurationFile", configurationFile);
        }

        private static void RunHaystackAnalyzer(string passingConfigurationFile, string failingConfigurationFile)
        {
            string[] args = new string[]
            {
                "--PassingConfigurationFile", passingConfigurationFile,
                "--FailingConfigurationFile", failingConfigurationFile
            };
            RunExecutable(Path.Combine(haystackBaseDirectory, @"Analysis\Haystack.Analyzer.exe"), args);
        }

        private static void RunExecutable(string exe, params string[] args)
        {
            ProcessRunner.ExecuteProcessInNewAppDomain(exe, args);
        }
    }
}
