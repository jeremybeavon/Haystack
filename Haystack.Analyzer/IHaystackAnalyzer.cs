using Haystack.Analyzer.ObjectModel;

namespace Haystack.Analyzer
{
    public interface IHaystackAnalyzer
    {
        void Analyze(IHaystackAnalysis haystackAnalysis);
    }
}
