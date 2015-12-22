using CommandLine;

namespace Haystack.Comparison
{
    public sealed class CommandLineOptions
    {
        [Option(Required = true)]
        public string PathToPassingHaystackTestOutput { get; set; }

        [Option(Required = true)]
        public string PathToFailingHaystackTestOutput { get; set; }
    }
}
