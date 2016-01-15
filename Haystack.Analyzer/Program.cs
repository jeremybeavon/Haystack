using CommandLine;
using Haystack.Analyzer.ObjectModel;
using Haystack.Diagnostics.Configuration;
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
            IHaystackConfiguration passingConfiguration = HaystackConfiguration.LoadFile(options.PassingConfigurationFile);
            IHaystackConfiguration failingConfiguration = HaystackConfiguration.LoadFile(options.FailingConfigurationFile);
            new HaystackAnalysis(passingConfiguration, failingConfiguration);
        }
    }
}
