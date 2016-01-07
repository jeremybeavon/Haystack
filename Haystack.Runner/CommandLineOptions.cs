using CommandLine;

namespace Haystack.Runner
{
    public sealed class CommandLineOptions
    {
        [Option("ConfigurationFile", Required = true)]
        public string ConfigurationFile { get; set; }
    }
}
