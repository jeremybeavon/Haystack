namespace Haystack.Diagnostics.CodeCoverage
{
    public interface ITestRunContext
    {
        string Exe { get; set; }

        string Arguments { get; set; }
    }
}
