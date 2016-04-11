using Haystack.Diagnostics.Amendments.Expressions;
using Haystack.Diagnostics.Configuration;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments
{
    public sealed class AssemblyAmender
    {
        private delegate Expression<Action<TInstance, string>> BeforePropertyGetFunc<TInstance>();

        private AssemblyAmender()
        {
        }

        public static void AmendAssembly(string assemblyFile, IAmendmentConfiguration configuration, string strongNameKey = null)
        {
            string pdbFile = Path.ChangeExtension(assemblyFile, "pdb");
            string backupAssemblyFile = assemblyFile + ".backup";
            string backupPdbFile = pdbFile + ".backup";
            File.Delete(backupAssemblyFile);
            File.Delete(backupPdbFile);
            File.Move(assemblyFile, backupAssemblyFile);
            if (File.Exists(pdbFile))
            {
                File.Move(pdbFile, backupPdbFile);
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            AssemblyDefinition assemblyDefinition = ReadAssembly(backupAssemblyFile, backupPdbFile);
            AmendAssembly(assembly, assemblyDefinition, configuration);
            WriteAssembly(assemblyDefinition, assemblyFile, pdbFile, strongNameKey);
        }


        internal static AssemblyDefinition ReadAssembly(string assemblyFile, string pdbFile)
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

        internal static void WriteAssembly(AssemblyDefinition assembly, string assemblyFile, string pdbFile, string strongNameKey)
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

        private static void AmendAssembly(Assembly assembly, AssemblyDefinition assemblyDefinition, IAmendmentConfiguration configuration)
        {
            IEnumerable<IBeforePropertyGetExpressionAmender> beforePropertyGetAmenders = null;
            foreach (TypeDefinition typeDefinition in assemblyDefinition.MainModule.Types)
            {
                Type type = assembly.GetType(typeDefinition.FullName, true);
                
            }
        }
    }
}
