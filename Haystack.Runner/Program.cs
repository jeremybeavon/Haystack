using CommandLine;
using Haystack.Core;
using Haystack.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;

namespace Haystack.Runner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            Parser.Default.ParseArgumentsStrict(args, options);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string haystackDiagnosticsDirectory = Path.Combine(baseDirectory, "Diagnostics");
            AppDomain.CurrentDomain.AddAssemblyResolveDirectory(haystackDiagnosticsDirectory);
            Trace.Listeners.Add(new ConsoleTraceListener());
            RunHaystackDiagnostics(options);
        }
        
        private static void RunHaystackDiagnostics(CommandLineOptions options)
        {
            HaystackRunner.RunHaystackDiagnostics(options.ConfigurationFile);
        }
    }
}
