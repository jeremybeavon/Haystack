using System;

namespace Haystack.Diagnostics.SourceControl
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class SourceControlProviderAttribute : Attribute
    {
        public SourceControlProviderAttribute(Type sourceControlProviderType)
        {
            SourceControlProviderType = sourceControlProviderType;
        }

        public Type SourceControlProviderType { get; private set; }
    }
}
