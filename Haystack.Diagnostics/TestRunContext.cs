namespace Haystack.Diagnostics
{
    public sealed class TestRunContext : ITestRunContext
    {
        public string Arguments { get; set; }

        public string Exe { get; set; }

        public string StandardOutputFile { get; set; }
    }
}
