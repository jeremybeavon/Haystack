using CommandLine;

namespace Haystack.Analyzer
{
    public sealed class CommandLineOptions
    {
        [Option("PassingConfigurationFile", Required = true)]
        public string PassingConfigurationFile { get; set; }

        [Option("FailingConfigurationFile", Required = true)]
        public string FailingConfigurationFile { get; set; }
    }
}
