namespace Haystack.Diagnostics.CodeCoverage
{
    public sealed class TestRunContext : ITestRunContext
    {
        public string Arguments { get; set; }

        public string Exe { get; set; }
    }
}
