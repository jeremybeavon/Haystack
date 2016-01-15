using CommandLine;
using Haystack.Core;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Haystack.Runner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            Parser.Default.ParseArgumentsStrict(args, options);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string haystackDiagnosticsDirectory = Path.Combine(baseDirectory, FrameworkVersion.Current, "Diagnostics");
            AppDomain.CurrentDomain.AddAssemblyResolveDirectory(haystackDiagnosticsDirectory);
            Trace.Listeners.Add(new ConsoleTraceListener());
            RunHaystackDiagnostics(options);
        }
        
        private static void RunHaystackDiagnostics(CommandLineOptions options)
        {
            IHaystackConfiguration configuration = HaystackInitializer.InitializeIfNecessary(options.ConfigurationFile);
            HaystackRunner.RunHaystackDiagnostics(configuration);
        }
    }
}
