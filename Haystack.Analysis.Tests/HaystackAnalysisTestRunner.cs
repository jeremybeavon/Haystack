using Haystack.Core;
using Haystack.Diagnostics;
using System;
using System.IO;

namespace Haystack.Analysis.Tests
{
    public static class HaystackAnalysisTestRunner
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string haystackBaseDirectory = Path.Combine(baseDirectory, @"..\..\Haystack", FrameworkVersion.Current);

        public static void RunHaystackAnalysis(string passingConfigurationFile, string failingConfigurationFile)
        {
            RelativePathResolver relativePathResolver = new RelativePathResolver(haystackBaseDirectory);
            relativePathResolver.ResolveIfNecessary(passingConfigurationFile, path => passingConfigurationFile = path);
            relativePathResolver.ResolveIfNecessary(failingConfigurationFile, path => failingConfigurationFile = path);
            RunHaystackRunner(passingConfigurationFile);
            RunHaystackRunner(failingConfigurationFile);
            string[] args = new string[]
            {
                "--PassingConfigurationFile", passingConfigurationFile,
                "--FailingConfigurationFile", failingConfigurationFile
            };
            RunExecutable(Path.Combine(haystackBaseDirectory, @"Analysis\Haystack.Analysis.exe"), args);
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
