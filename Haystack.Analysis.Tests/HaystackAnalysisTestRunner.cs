using Haystack.Core;
using Haystack.Core.IO;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using System;
using System.IO;

namespace Haystack.Analysis.Tests
{
    public static class HaystackAnalysisTestRunner
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string haystackBaseDirectory = 
            Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\Haystack", FrameworkVersion.Current));

        public static void RunHaystackAnalysis(string testBaseDirectory, string passingConfigurationFile, string failingConfigurationFile)
        {
            string testDirectory = InitializeTestDirectory(testBaseDirectory);
            passingConfigurationFile = InitializeConfigurationFile(testBaseDirectory, passingConfigurationFile);
            failingConfigurationFile = InitializeConfigurationFile(testBaseDirectory, failingConfigurationFile);
            RunHaystackRunner(passingConfigurationFile);
            RunHaystackRunner(failingConfigurationFile);
            string[] args = new string[]
            {
                "--PassingConfigurationFile", passingConfigurationFile,
                "--FailingConfigurationFile", failingConfigurationFile
            };
            RunExecutable(Path.Combine(haystackBaseDirectory, @"Analysis\Haystack.Analysis.exe"), args);
        }

        private static string InitializeTestDirectory(string testBaseDirectory)
        {
            string originalDirectory = Path.Combine(haystackBaseDirectory, testBaseDirectory);
            string testDirectory = Path.Combine(baseDirectory, testBaseDirectory);
            DirectoryCopy.CopyDirectory(originalDirectory, testBaseDirectory);
            return testDirectory;
        }

        private static string InitializeConfigurationFile(string testDirectory, string configurationFile)
        {
            configurationFile = Path.Combine(testDirectory, configurationFile);
            HaystackConfiguration haystackConfiguration = HaystackConfiguration.LoadText(File.ReadAllText(configurationFile));
            haystackConfiguration.HaystackBaseDirectory = haystackBaseDirectory;
            File.WriteAllText(configurationFile, haystackConfiguration.ToString());
            return configurationFile;
        }

        private static void RunHaystackRunner(string configurationFile)
        {
            RunExecutable(Path.Combine(haystackBaseDirectory, @"Runner\Haystack.Runner.exe"), "--ConfigurationFile", configurationFile);
        }

        private static void RunExecutable(string exe, params string[] args)
        {
            ProcessRunner.ExecuteProcessInNewAppDomain(exe, args);
        }
    }
}
