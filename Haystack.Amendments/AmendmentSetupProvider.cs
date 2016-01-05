using Haystack.Amendments.Properties;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Haystack.Amendments
{
    public class AmendmentSetupProvider
    {
        public const string ConfigurationKey = "haystackConfiguration";
        public const string AfterthoughtAmenderExeFileName = "Afterthought.Amender.exe";
        public const string AmendmentsDllFileName = "Haystack.Amendments.dll";

        private static readonly int codeBasePrefixLength = "file:///".Length;

        public AmendmentSetupProvider(string assemblyPath, HaystackConfiguration configuration, string strongNameKey = null)
        {
            AssemblyPath = assemblyPath;
            Configuration = configuration;
            StrongNameKey = strongNameKey;
            string currentLocation = Path.GetDirectoryName(GetAssemblyLocation(Assembly.GetExecutingAssembly()));
            AfterthoughtAmenderExe = Path.Combine(currentLocation, AfterthoughtAmenderExeFileName);
            AmendmentsDll = Path.Combine(currentLocation, AmendmentsDllFileName);
        }

        public string AssemblyPath { get; set; }

        public HaystackConfiguration Configuration { get; set; }

        public string StrongNameKey { get; set; }

        public string AfterthoughtAmenderExe { get; set; }

        public string AmendmentsDll { get; set; }

        public static void SetupIfNecessary(string assemblyPath, HaystackConfiguration configuration, string strongNameKey = null)
        {
            new AmendmentSetupProvider(assemblyPath, configuration, strongNameKey).SetupIfNecessary();
        }

        public void SetupIfNecessary()
        {
            if (!File.Exists(AssemblyPath) || IsAssemblySetUp(AssemblyPath))
            {
                return;
            }

            string assemblyName = Path.GetFileNameWithoutExtension(AssemblyPath);
            string baseDirectory = Path.GetDirectoryName(AssemblyPath);
            foreach (string file in Directory.GetFiles(baseDirectory, assemblyName + ".*"))
            {
                string fileName = Path.GetFileName(file);
                if (fileName != assemblyName + ".dll" && fileName != assemblyName + ".pdb")
                    File.Delete(file);
            }

            AmendAssembly();
            if (StrongNameKey != null)
            {
                IDictionary<string, string> properties = new Dictionary<string, string>
                {
                    { "AssemblyPath", AssemblyPath },
                    { "StrongNameKey", StrongNameKey }
                };
                MsBuildRunner.RunMsBuildXml(Resources.HaystackAfterthoughtSetup, properties);
            }
        }

        private static bool IsAssemblySetUp(string path)
        {
            return AssemblyDefinition.ReadAssembly(path).MainModule.AssemblyReferences.Any(assembly => assembly.Name == "Haystack.Diagnostics");
        }

        private static string GetAssemblyLocation(Assembly assembly)
        {
            return assembly.CodeBase.Substring(codeBasePrefixLength).Replace('/', '\\');
        }

        private void AmendAssembly()
        {
            if (!File.Exists(AfterthoughtAmenderExe))
            {
                Trace.WriteLine("Cannot set up Haystack diagnostics because {0} was not found.", AfterthoughtAmenderExe);
                return;
            }

            if (!File.Exists(AmendmentsDll))
            {
                Trace.WriteLine("Cannot set up Haystack diagnostics because {0} was not found.", AmendmentsDll);
                return;
            }

            AppDomainSetup appDomainSetup = new AppDomainSetup()
            {
                ApplicationBase = Path.GetDirectoryName(AfterthoughtAmenderExe),
                ConfigurationFile = AfterthoughtAmenderExe + ".config"
            };
            using (DisposableAppDomain appDomain = new DisposableAppDomain("Amender", appDomainSetup))
            { 
                appDomain.AppDomain.SetData(ConfigurationKey, Configuration.ToString());
                appDomain.AppDomain.ExecuteAssembly(AfterthoughtAmenderExe, new string[] { AssemblyPath, AmendmentsDll });
            }
        }
    }
}
