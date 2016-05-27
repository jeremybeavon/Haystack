using Haystack.Analysis.Configuration;
using Haystack.Analysis.ObjectModel;
using System.IO;

namespace Haystack.Analysis
{
    public static class HaystackAnalyzer
    {
        public static void RunHaystackAnalyzer(IHaystackAnalysisConfiguration configuration)
        {
            string haystackAnalysisOutputFile = Path.Combine(configuration.OutputDirectory, "haystackAnalysis");
            HaystackAnalysis analysis =
                File.Exists(haystackAnalysisOutputFile) ?
                HaystackAnalysisProvider.Load(haystackAnalysisOutputFile) :
                new HaystackAnalysis(configuration);
            HaystackAnalysisProvider.Save(haystackAnalysisOutputFile, analysis);
        }
    }
}
