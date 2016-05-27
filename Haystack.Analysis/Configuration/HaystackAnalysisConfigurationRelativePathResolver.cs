using Haystack.Core;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analysis.Configuration
{
    internal sealed class HaystackAnalysisConfigurationRelativePathResolver
    {
        private readonly string baseDirectory;
        private readonly RelativePathResolver relativePathResolver;

        private HaystackAnalysisConfigurationRelativePathResolver(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
            relativePathResolver = new RelativePathResolver(baseDirectory);
        }

        public static void ResolveRelativePaths(HaystackAnalysisConfiguration configuration, string baseDirectory)
        {
            new HaystackAnalysisConfigurationRelativePathResolver(baseDirectory).ResolveRelativePaths(configuration);
        }

        private void ResolveRelativePaths(HaystackAnalysisConfiguration configuration)
        {
            ResolveIfNecessary(configuration.HaystackBaseDirectory, path => configuration.HaystackBaseDirectory = path);
            ResolveIfNecessary(configuration.FailingTestOutputDirectory, path => configuration.FailingTestOutputDirectory = path);
            ResolveIfNecessary(configuration.PassingTestOutputDirectory, path => configuration.PassingTestOutputDirectory = path);
            if (string.IsNullOrWhiteSpace(configuration.OutputDirectory))
            {
                configuration.OutputDirectory = baseDirectory;
            }
            else
            {
                ResolveIfNecessary(configuration.OutputDirectory, path => configuration.OutputDirectory = path);
            }
        }

        private void ResolveIfNecessary(string path, Action<string> updateAction)
        {
            relativePathResolver.ResolveIfNecessary(path, updateAction);
        }
    }
}
