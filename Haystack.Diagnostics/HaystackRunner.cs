using Haystack.Diagnostics.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Haystack.Diagnostics
{
    public static class HaystackRunner
    {
        public static void RunHaystackDiagnostics(HaystackConfiguration configuration)
        {
            IEnumerable<string> referencedDirectories = GetReferencedDirectories(configuration);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assemblyFile = new AssemblyName(args.Name).Name + ".dll";
                string assemblyPath = referencedDirectories
                    .Select(referencedDirectory => Path.Combine(referencedDirectory, assemblyFile))
                    .FirstOrDefault(file => File.Exists(file));
                return assemblyPath == null ? null : Assembly.LoadFrom(assemblyPath);
            };
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> GetReferencedDirectories(HaystackConfiguration configuration)
        {
            List<string> referencedDirectories = new List<string>();
            string baseDirectory = Assembly.GetExecutingAssembly().AssemblyBaseDirectory();
            referencedDirectories.Add(Path.Combine(baseDirectory, "References"));
            return referencedDirectories;
        }
    }
}
