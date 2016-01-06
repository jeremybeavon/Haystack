namespace Haystack.Diagnostics.CodeCoverage
{
    public sealed class CodeCoverageProvider
    {
        public CodeCoverageProvider(object provider, ICodeCoverageContext context)
        {
            Provider = provider;
            Context = context;
        }

        public object Provider { get; private set; }

        public ICodeCoverageContext Context { get; private set; }
    }
}
