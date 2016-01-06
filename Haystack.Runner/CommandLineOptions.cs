using CommandLine;

namespace Haystack.Runner
{
    public sealed class CommandLineOptions
    {
        [Option(Required = true)]
        public string ConfigurationFile { get; set; }
    }
}
