using System;
using System.IO;
using System.Reflection;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Core
#endif
{
    public static class AssemblyExtensions
    {
        private static readonly int codeBasePrefixLength = "file:///".Length;

        public static string AssemblyBaseDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.CodeBase.Substring(codeBasePrefixLength).Replace('/', '\\'));
        }
    }
}
