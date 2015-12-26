using Haystack.Diagnostics;
using Haystack.Amendments.Properties;
using Mono.Cecil;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Haystack.Amendments
{
    public static class AfterthoughtSetupProvider
    {
        public static void SetupDiagnosticsIfNecessary(string baseDirectory, string assemblyName, string strongNameKey = null)
        {
            string assemblyPath = Path.Combine(baseDirectory, assemblyName + ".dll");
            if (!File.Exists(assemblyPath) || IsAssemblySetUp(assemblyPath))
                return;

            foreach (string file in Directory.GetFiles(baseDirectory, assemblyName + ".*"))
            {
                string fileName = Path.GetFileName(file);
                if (fileName != assemblyName + ".dll" && fileName != assemblyName + ".pdb")
                    File.Delete(file);
            }

            IDictionary<string, string> properties = GetProperties(baseDirectory, assemblyPath, strongNameKey);
            if (properties != null)
            {
                MsBuildRunner.RunMsBuildXml(Resources.HaystackAfterthoughtSetup, properties);
            }
        }

        private static bool IsAssemblySetUp(string path)
        {
            return AssemblyDefinition.ReadAssembly(path).MainModule.AssemblyReferences.Any(assembly => assembly.Name == "Haystack.Diagnostics");
        }

        private static IDictionary<string, string> GetProperties(string baseDirectory, string assemblyPath, string strongNameKey)
        {
            string currentLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string afterthoughtAmenderExe = Path.Combine(currentLocation, "Afterthought.Amender.exe");
            if (!File.Exists(afterthoughtAmenderExe))
            {
                Trace.WriteLine("Cannot set up Haystack diagnostics because {0} was not found.", afterthoughtAmenderExe);
                return null;
            }

            string amendmentsDll = Path.Combine(currentLocation, "Haystack.Amendments.dll");
            if (!File.Exists(amendmentsDll))
            {
                Trace.WriteLine("Cannot set up Haystack diagnostics because {0} was not found.", amendmentsDll);
                return null;
            }
            
            return new Dictionary<string, string>
            {
                {"AssemblyPath", assemblyPath},
                {"AfterthoughtAmenderExe", afterthoughtAmenderExe},
                {"AmendmentsDll", amendmentsDll},
                {"StrongNameKey", strongNameKey ?? string.Empty}
            };
        }
    }
}
