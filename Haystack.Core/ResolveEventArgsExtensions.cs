using System;
using System.IO;
using System.Reflection;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Core
#endif
{
    public static class ResolveEventArgsExtensions
    {
        public static string AssemblyName(this ResolveEventArgs args)
        {
            return new AssemblyName(args.Name).Name;
        }

        public static Assembly ResolveAssembly(this ResolveEventArgs args, string assemblyFile)
        {
            return args.AssemblyName() == Path.GetFileNameWithoutExtension(assemblyFile) ? Assembly.LoadFrom(assemblyFile) : null;
        }

        public static Assembly ResolveDiagnosticsAssembly(this ResolveEventArgs args, string directory)
        {
            return args.ResolveAssembly(Path.Combine(directory, "Haystack.Diagnostics.dll"));
        }
    }
}
