using Haystack.Analysis.ObjectModel;

namespace Haystack.Analysis
{
    public interface IHaystackAnalyzer
    {
        void Analyze(IHaystackAnalysis haystackAnalysis);
    }
}
