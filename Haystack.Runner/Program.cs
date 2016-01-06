using CommandLine;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using System;
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
            AppDomain.CurrentDomain.AssemblyResolve += DiagnosticsAssemblyResolve;
            RunHaystackDiagnostics(options);
        }

        private static Assembly DiagnosticsAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (new AssemblyName(args.Name).Name == "Haystack.Diagnostics")
            {
                return Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Haystack.Diagnostics.dll"));
            }

            return null;
        }

        private static void RunHaystackDiagnostics(CommandLineOptions options)
        {
            HaystackConfiguration configuration = HaystackConfiguration.LoadFile(options.ConfigurationFile);
            HaystackRunner.RunHaystackDiagnostics(configuration);
        }
    }
}
