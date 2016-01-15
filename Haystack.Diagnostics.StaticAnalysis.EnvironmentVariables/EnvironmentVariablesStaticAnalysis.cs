using Haystack.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Diagnostics.StaticAnalysis.EnvironmentVariables
{
    public sealed class EnvironmentVariablesStaticAnalysis : IStaticAnalysis
    {
        public StaticAnalysisOutput RunInitialAnalysis(IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            return new StaticAnalysisOutput()
            {
                Before = GetEnvironmentVariables(includedItems)
            };
        }

        public void RunFinalAnalysis(StaticAnalysisOutput output, IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            output.After = GetEnvironmentVariables(includedItems);
        }

        private static List<string> GetEnvironmentVariables(IEnumerable<string> includedItems)
        {
            return includedItems == null ? new List<string>() :
                includedItems.Select(item => item + " = " + Environment.GetEnvironmentVariable(item)).ToList();
        }
    }
}
