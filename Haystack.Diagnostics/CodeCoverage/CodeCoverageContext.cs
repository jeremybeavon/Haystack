namespace Haystack.Diagnostics.CodeCoverage
{
    public sealed class CodeCoverageContext : ICodeCoverageContext
    {
        public CodeCoverageContext(string codeCoverageFilter, string outputDirectory)
        {
            CodeCoverageFilter = codeCoverageFilter;
            OutputDirectory = outputDirectory;
        }

        public string CodeCoverageFilter { get; private set; }

        public string OutputDirectory { get; private set; }
    }
}
