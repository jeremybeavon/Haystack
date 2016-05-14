using Haystack.Core;
using System;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class TypeConfiguration
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
