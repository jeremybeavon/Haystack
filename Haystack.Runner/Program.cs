using CommandLine;
using System;
using System.IO;

namespace Haystack.Runner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            Parser.Default.ParseArgumentsStrict(args, options);
            string outputDirectory = Path.Combine(options.TestDirectory, "Haystack.Output");
            Directory.CreateDirectory(outputDirectory);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            (new CodeCoverageRunner()
            {
                PathToOpenCover = Path.Combine(baseDirectory, @"OpenCover\OpenCover.Console.exe"),
                PathToReportGenerator = Path.Combine(baseDirectory, @"ReportGenerator\ReportGenerator.exe"),
                PathToTestRunner = GetPathToTestRunner(options.TestRunner, baseDirectory),
                TestRunnerArguments = options.TestRunnerArguments,
                PathToCodeCoverageXmlFile = Path.Combine(outputDirectory, "clr_code_coverage.xml"),
                PathToCodeCoverageReportDirectory = Path.Combine(outputDirectory, "clr_code_coverage"),
                CodeCoverageFilter = options.CodeCoverageFilter
            }).RunCodeCoverage();
        }

        private static string GetPathToTestRunner(string testRunner, string baseDirectory)
        {
            switch (testRunner.ToLower())
            {
                case "nunit.2.6.1":
                    return Path.Combine(baseDirectory, @"NUnit\2.6.1\nunit-console-x86.exe");
                case "nunit.2.6.2":
                    return Path.Combine(baseDirectory, @"NUnit\2.6.2\nunit-console-x86.exe");
                case "nunit.2.6.3":
                    return Path.Combine(baseDirectory, @"NUnit\2.6.3\nunit-console-x86.exe");
                case "nunit.2.6.4":
                    return Path.Combine(baseDirectory, @"NUnit\2.6.4\nunit-console-x86.exe");
                case "nunit.3.0.0":
                    return Path.Combine(baseDirectory, @"NUnit\3.0.0\nunit3-console.exe");
                case "nunit.3.0.1":
                    return Path.Combine(baseDirectory, @"NUnit\3.0.1\nunit3-console.exe");
                default:
                    throw new InvalidOperationException(string.Format("testRunner was not found: {0}", testRunner));
            }
        }
    }
}
