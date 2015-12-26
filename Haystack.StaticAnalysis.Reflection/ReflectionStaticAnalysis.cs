using Haystack.Diagnostics;
using System;
using System.Collections.Generic;

namespace Haystack.StaticAnalysis.Reflection
{
    public sealed class ReflectionStaticAnalysis : IStaticAnalysis
    {
        public StaticAnalysisOutput RunInitialAnalysis(IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            throw new NotImplementedException();
        }

        public void RunFinalAnalysis(StaticAnalysisOutput output, IEnumerable<string> includedItems, IEnumerable<string> excludedItems)
        {
            throw new NotImplementedException();
        }
    }
}
