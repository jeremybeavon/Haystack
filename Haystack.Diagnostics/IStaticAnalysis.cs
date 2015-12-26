using System.Collections.Generic;

namespace Haystack.Diagnostics
{
    public interface IStaticAnalysis
    {
        StaticAnalysisOutput RunInitialAnalysis(IEnumerable<string> includedItems, IEnumerable<string> excludedItems);

        void RunFinalAnalysis(StaticAnalysisOutput output, IEnumerable<string> includedItems, IEnumerable<string> excludedItems);
    }
}
