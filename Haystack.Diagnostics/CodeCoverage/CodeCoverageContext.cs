namespace Haystack.Diagnostics.CodeCoverage
{
    public sealed class CodeCoverageContext : ICodeCoverageContext
    {
        public CodeCoverageContext(string codeCoverageFilter)
        {
            CodeCoverageFilter = codeCoverageFilter;
        }

        public string CodeCoverageFilter { get; private set; }
    }
}
