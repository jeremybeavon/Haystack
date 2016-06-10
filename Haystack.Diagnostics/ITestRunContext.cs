namespace Haystack.Diagnostics
{
    public interface ITestRunContext
    {
        string Exe { get; set; }

        string Arguments { get; set; }

        string StandardOutputFile { get; set; }
        
        string AssemblyToTest { get; }
    }
}
