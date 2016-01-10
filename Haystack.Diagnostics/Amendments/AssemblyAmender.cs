using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Pdb;

namespace Haystack.Diagnostics.Amendments
{
    public static class AssemblyAmender
    {
        public static void AddAssemblyAttribute(string assemblyFile, Type attributeType, string strongNameKey = null)
        {
            string pdbFile = Path.ChangeExtension(assemblyFile, "pdb");
            AssemblyDefinition assembly = ReadAssembly(assemblyFile, pdbFile);
            if (assembly.CustomAttributes.Any(attribute => attribute.Constructor.DeclaringType.FullName == attributeType.FullName))
            {
                return;
            }

            assembly.CustomAttributes.Add(new CustomAttribute(assembly.MainModule.Import(attributeType.GetConstructor(Type.EmptyTypes))));
            WriteAssembly(assembly, assemblyFile, pdbFile, strongNameKey);
        }

        private static AssemblyDefinition ReadAssembly(string assemblyFile, string pdbFile)
        {
            if (File.Exists(pdbFile))
            {
                using (Stream pdbStream = File.OpenRead(pdbFile))
                {
                    ReaderParameters readerParameters = new ReaderParameters()
                    {
                        ReadSymbols = true,
                        SymbolReaderProvider = new PdbReaderProvider(),
                        SymbolStream = pdbStream
                    };
                    return AssemblyDefinition.ReadAssembly(assemblyFile, readerParameters);
                }
            }

            return AssemblyDefinition.ReadAssembly(assemblyFile);
        }

        private static void WriteAssembly(AssemblyDefinition assembly, string assemblyFile, string pdbFile, string strongNameKey)
        {
            StrongNameKeyPair key = null;
            if (!string.IsNullOrWhiteSpace(strongNameKey))
            {
                using (FileStream stream = File.OpenRead(strongNameKey))
                {
                    key = new StrongNameKeyPair(stream);
                }
            }

            WriterParameters writerParameters = new WriterParameters()
            {
                StrongNameKeyPair = key,
                WriteSymbols = File.Exists(pdbFile),
                SymbolWriterProvider = new PdbWriterProvider()
            };
            assembly.Write(assemblyFile, writerParameters);
        }
    }
}
