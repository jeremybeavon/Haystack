using Haystack.Diagnostics.Configuration;

namespace Haystack.Analysis
{
    public interface IHaystackAnalyzer
    {
        void Analyze(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration);
    }
}
