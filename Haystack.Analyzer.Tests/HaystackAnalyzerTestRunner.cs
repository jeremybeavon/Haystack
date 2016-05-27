using Haystack.Analysis;
using Haystack.Analysis.ObjectModel;
using Haystack.Bootstrap;
using Haystack.Core.IO;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace Haystack.Analyzer.Tests
{
    public static class HaystackAnalyzerTestRunner
    {
        private const string ConfigurationFileName = HaystackConfigurationFile.ConfigurationFileName;
        private const string LaunchDebuggerFileName = HaystackInitializer.LaunchDebuggerFileName;
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string haystackBaseDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\Haystack"));
        private static readonly string haystackRunnerDirectory =
            Path.Combine(haystackBaseDirectory, "Runner", FrameworkVersion.Current);
        private static readonly string haystackExamplesDirectory =
            Path.Combine(haystackBaseDirectory, "Examples", FrameworkVersion.Current);

        public static HaystackAnalysis RunHaystackAnalyzer(string exampleDirectory)
        {
            string testDirectory = InitializeTestDirectory(exampleDirectory);
            string passingConfigurationFile = InitializeConfigurationFile(testDirectory, true);
            string failingConfigurationFile = InitializeConfigurationFile(testDirectory, false);
            RunHaystackRunner(passingConfigurationFile);
            RunHaystackRunner(failingConfigurationFile);
            string analysisConfigurationFile = Path.Combine(testDirectory, "haystackanalysis.config.xml");
            string[] args = new string[]
            {
                "--ConfigurationFile", analysisConfigurationFile
            };
            RunExecutable(Path.Combine(haystackBaseDirectory, @"Analysis\Haystack.Analyzer.exe"), args);
            return HaystackAnalysisProvider.Load(Path.Combine(testDirectory, "haystackAnalysis"));
        }

        private static string InitializeTestDirectory(string exampleDirectory)
        {
            string originalDirectory = Path.Combine(haystackExamplesDirectory, exampleDirectory);
            string testDirectory = Path.Combine(baseDirectory, "Examples", exampleDirectory);
            DirectoryCopy.CopyDirectory(originalDirectory, testDirectory);
            return testDirectory;
        }

        private static string InitializeConfigurationFile(string testDirectory, bool isPassing)
        {
            string configurationFile = Path.Combine(testDirectory, isPassing ? "Passing" : "Failing", ConfigurationFileName);
            HaystackConfiguration haystackConfiguration = HaystackConfiguration.LoadText(File.ReadAllText(configurationFile));
            haystackConfiguration.HaystackBaseDirectory = haystackBaseDirectory;
            File.WriteAllText(configurationFile, haystackConfiguration.ToString());
            return configurationFile;
        }

        private static void RunHaystackRunner(string configurationFile)
        {
            if (Debugger.IsAttached)
            {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(configurationFile), LaunchDebuggerFileName), string.Empty);
            }

            string[] args = new string[] { "--ConfigurationFile", configurationFile };
            RunExecutable(Path.Combine(haystackRunnerDirectory, @"Haystack.Runner.exe"), args);
        }
        
        private static void RunExecutable(string exe, params string[] args)
        {
            ProcessRunner.ExecuteProcessInNewAppDomain(exe, args);
        }
    }
}
