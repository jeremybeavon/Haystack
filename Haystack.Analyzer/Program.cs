using CommandLine;
using Haystack.Analysis;
using Haystack.Analysis.Configuration;
using System.Diagnostics;

namespace Haystack.Analyzer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            Parser.Default.ParseArgumentsStrict(args, options);
            Trace.Listeners.Add(new ConsoleTraceListener());
            RunHaystackAnalysis(options);
        }

        private static void RunHaystackAnalysis(CommandLineOptions options)
        {
            IHaystackAnalysisConfiguration configuration = HaystackAnalysisConfiguration.LoadFile(options.ConfigurationFile);
            HaystackAnalyzer.RunHaystackAnalyzer(configuration);
        }
    }
}
