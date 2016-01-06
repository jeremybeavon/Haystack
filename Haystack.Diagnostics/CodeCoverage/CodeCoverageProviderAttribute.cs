using System;

namespace Haystack.Diagnostics.CodeCoverage
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class CodeCoverageProviderAttribute : Attribute
    {
        public CodeCoverageProviderAttribute(Type codeCoverageProviderType)
        {
            CodeCoverageProviderType = codeCoverageProviderType;
        }

        public Type CodeCoverageProviderType { get; private set; }
    }
}
