using System.Collections.Generic;
using System.Linq;
using Haystack.Diagnostics;
using Haystack.Diagnostics.StaticAnalysis.FileSystem.Properties;
using Microsoft.Build.Execution;

namespace Haystack.Diagnostics.StaticAnalysis.FileSystem
{
    public sealed class FileSystemStaticAnalysis : IStaticAnalysis
    {
        public StaticAnalysisOutput RunInitialAnalysis(IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            return new StaticAnalysisOutput()
            {
                Before = FindFiles(includedItems, excludedItems)
            };
        }

        public void RunFinalAnalysis(StaticAnalysisOutput output, IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            output.After = FindFiles(includedItems, excludedItems);
        }

        private static List<string> FindFiles(IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            IDictionary<string, string> properties = new Dictionary<string, string>()
            {
                { "IncludedFiles", string.Join(";", includedItems) },
                { "ExcludedFiles", string.Join(";", excludedItems) }
            };
            BuildResult result = MsBuildRunner.RunMsBuildXmlWithResult(Resources.FileSystem, properties);
            return result.ResultsByTarget["FindFiles"].Items.Select(item => item.ItemSpec).ToList();
        }
    }
}
