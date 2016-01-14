using CommandLine;
using Haystack.Analysis.ObjectModel;
using Haystack.Core;
using Haystack.Diagnostics.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace Haystack.Analysis
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            Parser.Default.ParseArgumentsStrict(args, options);
            string haystackBaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
            AppDomain.CurrentDomain.AddAssemblyResolveDirectory(haystackBaseDirectory);
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
