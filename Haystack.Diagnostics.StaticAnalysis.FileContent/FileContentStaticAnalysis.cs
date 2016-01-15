using System;
using System.Collections.Generic;
using Haystack.Diagnostics;
using System.IO;
using System.Linq;

namespace Haystack.Diagnostics.StaticAnalysis.FileContent
{
    public sealed class FileContentStaticAnalysis : IStaticAnalysis
    {
        public StaticAnalysisOutput RunInitialAnalysis(IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            return new StaticAnalysisOutput()
            {
                Before = GetFileContent(includedItems)
            };
        }

        public void RunFinalAnalysis(StaticAnalysisOutput output, IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            output.After = GetFileContent(includedItems);
        }

        private static List<string> GetFileContent(IEnumerable<string> includedItems)
        {
            string fileName = includedItems.SingleOrDefault();
            if (fileName == null)
            {
                throw new ArgumentException("includedItems must contain exactly one file.", "includedItems");
            }

            return File.Exists(fileName) ? new List<string>(new string[] { File.ReadAllText(fileName) }) : null;
        }
    }
}
