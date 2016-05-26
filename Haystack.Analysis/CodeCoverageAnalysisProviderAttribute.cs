using System;

namespace Haystack.Analysis
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class CodeCoverageAnalysisProviderAttribute : Attribute
    {
        public CodeCoverageAnalysisProviderAttribute(Type codeCoverageAnalysisProviderType)
        {
            CodeCoverageAnalysisProviderType = codeCoverageAnalysisProviderType;
        }

        public Type CodeCoverageAnalysisProviderType { get; private set; }
    }
}
