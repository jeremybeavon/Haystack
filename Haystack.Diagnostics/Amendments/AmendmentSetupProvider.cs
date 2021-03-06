﻿using Haystack.Diagnostics.Configuration;
using Haystack.Core;
using Haystack.Diagnostics.Properties;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public sealed class AmendmentSetupProvider
    {
        public const string ConfigurationKey = "haystackConfiguration";
        public const string AfterthoughtAmenderExeFileName = "Afterthought.Amender.exe";
        public const string AmendmentsDllFileName = "Haystack.Diagnostics.Amendments.Afterthought.dll";
        
        public AmendmentSetupProvider(string assemblyToAmend, IHaystackConfiguration configuration)
        {
            AssemblyToAmend = assemblyToAmend;
            Configuration = configuration;
            string currentLocation = Assembly.GetExecutingAssembly().AssemblyBaseDirectory();
            AfterthoughtAmenderExe = Path.Combine(currentLocation, @"Amendments\Afterthought", AfterthoughtAmenderExeFileName);
            AmendmentsDll = Path.Combine(currentLocation, @"Amendments\Afterthought", AmendmentsDllFileName);
        }

        public string AssemblyToAmend { get; set; }

        public IHaystackConfiguration Configuration { get; set; }

        public string AfterthoughtAmenderExe { get; set; }

        public string AmendmentsDll { get; set; }

        public static void SetupIfNecessary(string assemblyToAmend, IHaystackConfiguration configuration)
        {
            new AmendmentSetupProvider(assemblyToAmend, configuration).SetupIfNecessary();
        }

        public void SetupIfNecessary()
        {
            if (!File.Exists(AssemblyToAmend))
            {
                Trace.WriteLine(string.Format("Skipping amendments because file not found: {0}", AssemblyToAmend));
                return;
            }

            if (IsAssemblySetUp(AssemblyToAmend))
            {
                Trace.WriteLine(string.Format("Skipping amendments because amendments are already applied: {0}", AssemblyToAmend));
                return;
            }

            CleanUpDirectory();
            AmendAssembly();
        }

        private static bool IsAssemblySetUp(string path)
        {
            return AssemblyDefinition.ReadAssembly(path).MainModule.AssemblyReferences.Any(assembly => assembly.Name == "Haystack.Bootstrap");
        }
        
        private void CleanUpDirectory()
        {
            string assemblyName = Path.GetFileNameWithoutExtension(AssemblyToAmend);
            string baseDirectory = Path.GetDirectoryName(AssemblyToAmend);
            foreach (string file in Directory.GetFiles(baseDirectory, assemblyName + ".*"))
            {
                string fileName = Path.GetFileName(file);
                if (Path.GetFileNameWithoutExtension(file) == assemblyName &&
                    fileName != assemblyName + ".dll" &&
                    fileName != assemblyName + ".pdb")
                {
                    File.Delete(file);
                }
            }
        }

        private void AmendAssembly()
        {
            if (!File.Exists(AfterthoughtAmenderExe))
            {
                Trace.WriteLine(string.Format("Cannot set up Haystack diagnostics because {0} was not found.", AfterthoughtAmenderExe));
                return;
            }

            if (!File.Exists(AmendmentsDll))
            {
                Trace.WriteLine(string.Format("Cannot set up Haystack diagnostics because {0} was not found.", AmendmentsDll));
                return;
            }

            AppDomainSetup appDomainSetup = new AppDomainSetup()
            {
                ApplicationBase = Path.GetDirectoryName(AfterthoughtAmenderExe),
                ConfigurationFile = AfterthoughtAmenderExe + ".config"
            };
            using (DisposableAppDomain appDomain = new DisposableAppDomain("Amender", appDomainSetup))
            {
                CrossDomainConsoleProvider.InitializeConsole(appDomain.AppDomain);
                appDomain.AppDomain.SetData(ConfigurationKey, Configuration.ToString());
                appDomain.AppDomain.AddAssemblyResolveDirectory(Assembly.GetExecutingAssembly().AssemblyBaseDirectory());
                int result = appDomain.AppDomain.ExecuteAssembly(AfterthoughtAmenderExe, new string[] { AssemblyToAmend, AmendmentsDll });
                if (result != 0)
                {
                    throw new InvalidOperationException("Assembly amendment failed.");
                }
            }
            
            ReSignAssemblyIfNecessary();
        }

        private void ReSignAssemblyIfNecessary()
        {
            if (Configuration.Amendments.StrongNameKeyFile != null)
            {
                IDictionary<string, string> properties = new Dictionary<string, string>
                {
                    { "AssemblyPath", AssemblyToAmend },
                    { "StrongNameKey", Configuration.Amendments.StrongNameKeyFile }
                };
                MsBuildRunner.RunMsBuildXml(Resources.AmendmentsStrongNameSetup, properties);
            }
        }
    }
}
