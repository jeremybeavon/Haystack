using Haystack.Core;
using System;
using System.IO;

namespace Haystack.Analysis.Tests
{
    public static class HaystackAnalysisTestRunner
    {
        private static readonly string haystackBaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Haystack");

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
            AppDomainSetup setup = new AppDomainSetup()
            {
                ApplicationBase = Path.GetDirectoryName(exe)
            };
            using (DisposableAppDomain appDomain = new DisposableAppDomain(Path.GetFileNameWithoutExtension(exe), setup))
            {
                CrossDomainConsoleProvider.InitializeConsole(appDomain.AppDomain);
                appDomain.AppDomain.ExecuteAssembly(exe, args);
            }
        }
    }
}
