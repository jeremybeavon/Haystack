using Haystack.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Haystack.Diagnostics.Configuration
{
    internal sealed class HaystackConfigurationRelativePathResolver
    {
        private readonly string baseDirectory;
        private readonly RelativePathResolver relativePathResolver;

        private HaystackConfigurationRelativePathResolver(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
            relativePathResolver = new RelativePathResolver(baseDirectory);
        }

        public static void ResolveRelativePaths(HaystackConfiguration configuration, string baseDirectory)
        {
            new HaystackConfigurationRelativePathResolver(baseDirectory).ResolveRelativePaths(configuration);
        }

        private void ResolveRelativePaths(HaystackConfiguration configuration)
        {
            ResolveIfNecessary(configuration.OutputDirectory, path => configuration.OutputDirectory = path);
            ResolveIfNecessary(configuration.HaystackBaseDirectory, path => configuration.HaystackBaseDirectory = path);
            if (configuration.Amendments != null)
            {
                ResolveIfNecessary(configuration.Amendments.AssembliesToAmend);
            }

            if (configuration.Runner != null)
            {
                RunnerConfiguration runner = configuration.Runner;
                ResolveUsingFunctionIfNecessary(runner.RunnerArguments, path => runner.RunnerArguments = path);
                ResolveUsingFunctionIfNecessary(runner.AssemblyToTest, path => runner.AssemblyToTest = path);
                ResolveIfNecessary(runner.CleanUpTestMethod);
                ResolveIfNecessary(runner.CleanUpTestSuite);
                ResolveIfNecessary(runner.InitializeTestFramework);
                ResolveIfNecessary(runner.InitializeTestMethod);
                ResolveIfNecessary(runner.InitializeTestSuite);
            }
        }

        private void ResolveIfNecessary(IEnumerable<TypeConfiguration> types)
        {
            if (types != null)
            {
                foreach (TypeConfiguration type in types)
                {
                    ResolveIfNecessary(type);
                }
            }
        }

        private void ResolveIfNecessary(TypeConfiguration type)
        {
            if (type != null)
            {
                ResolveUsingFunctionIfNecessary(type.AssemblyFile, path => type.AssemblyFile = path);
            }
        }

        private void ResolveIfNecessary(IList<string> paths)
        {
            relativePathResolver.ResolveIfNecessary(paths);
        }

        private void ResolveIfNecessary(string path, Action<string> updateAction)
        {
            relativePathResolver.ResolveIfNecessary(path, updateAction);
        }

        private void ResolveUsingFunctionIfNecessary(string text, Action<string> updateAction)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                const string pattern = @"\$\(CurrentDirectory\((?<Path>[^\)]+)\)\)";
                updateAction(Regex.Replace(text, pattern, match => Path.GetFullPath(Path.Combine(baseDirectory, match.Groups["Path"].Value))));
            }
        }
    }
}
