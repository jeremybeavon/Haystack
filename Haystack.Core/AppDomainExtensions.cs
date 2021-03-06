﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Core
#endif
{
    public static class AppDomainExtensions
    {
        public static void AddAssemblyResolveDirectory(this AppDomain appDomain, params string[] referencedDirectories)
        {
            appDomain.AssemblyResolve += new ReferencedDirectories(referencedDirectories).ResolveAssembly;
        }

        [Serializable]
        private sealed class ReferencedDirectories
        {
            private string[] referencedDirectories;

            public ReferencedDirectories(string[] referencedDirectories)
            {
                this.referencedDirectories = referencedDirectories;
            }

            public Assembly ResolveAssembly(object sender, ResolveEventArgs args)
            {
                string assemblyFile = new AssemblyName(args.Name).Name + ".dll";
                string assemblyPath = referencedDirectories
                    .Select(referencedDirectory => Path.Combine(referencedDirectory, assemblyFile))
                    .FirstOrDefault(file => File.Exists(file));
                return assemblyPath == null ? null : Assembly.LoadFrom(assemblyPath);
            }
        }
    }
}
