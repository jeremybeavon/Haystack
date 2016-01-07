﻿using CommandLine;
using Haystack.Core;
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
            AppDomain.CurrentDomain.AssemblyResolve += (sender, resolveArgs) =>
                resolveArgs.ResolveDiagnosticsAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".."));
            RunHaystackDiagnostics(options);
        }
        
        private static void RunHaystackDiagnostics(CommandLineOptions options)
        {
            IHaystackConfiguration configuration = HaystackInitializer.InitializeIfNecessary(options.ConfigurationFile);
            HaystackRunner.RunHaystackDiagnostics(configuration);
        }
    }
}
