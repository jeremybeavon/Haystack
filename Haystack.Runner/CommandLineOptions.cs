using CommandLine;

namespace Haystack.Runner
{
    public sealed class CommandLineOptions
    {
        [Option(Required = true)]
        public string TestRunner { get; set; }

        [Option(Required = true)]
        public string TestDirectory { get; set; }

        [Option(Required = true)]
        public string TestRunnerArguments { get; set; }
        
        [Option(Required = true)]
        public string CodeCoverageFilter { get; set; }
    }
}
