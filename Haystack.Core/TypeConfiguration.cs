using System;

namespace Haystack.Core
{
    public sealed class TypeConfiguration : ITypeConfiguration
    {
        public TypeConfiguration()
        {
        }

        public TypeConfiguration(Type type)
        {
            AssemblyFile = type.Assembly.AssemblyFilePath();
            Type = type.FullName;
        }

        public string AssemblyFile { get; set; }

        public string Type { get; set; }
    }
}
