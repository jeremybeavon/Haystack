using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Pdb;

namespace Haystack.Diagnostics.Amendments
{
    public static class AssemblyAttributeAmender
    {
        public static void AddAssemblyAttribute(string assemblyFile, Type attributeType, string strongNameKey = null)
        {
            string pdbFile = Path.ChangeExtension(assemblyFile, "pdb");
            AssemblyDefinition assembly = AssemblyAmender.ReadAssembly(assemblyFile, pdbFile);
            if (assembly.CustomAttributes.Any(attribute => attribute.Constructor.DeclaringType.FullName == attributeType.FullName))
            {
                return;
            }

            assembly.CustomAttributes.Add(new CustomAttribute(assembly.MainModule.Import(attributeType.GetConstructor(Type.EmptyTypes))));
            AssemblyAmender.WriteAssembly(assembly, assemblyFile, pdbFile, strongNameKey);
        }
    }
}
