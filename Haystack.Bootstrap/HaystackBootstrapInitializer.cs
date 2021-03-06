﻿using Haystack.Diagnostics;
using System;
using System.IO;
using System.Reflection;

namespace Haystack.Bootstrap
{
    public static class HaystackBootstrapInitializer
    {
        private static readonly object initializeLock = new object();
        private static bool isInitialized;

        public static void InitializeIfNecessary()
        {
            if (isInitialized)
            {
                return;
            }

            lock (initializeLock)
            {
                if (isInitialized)
                {
                    return;
                }

                Initialize();
                isInitialized = true;
            }
        }

        private static void Initialize()
        {
            string configurationFile = HaystackConfigurationFile.GetHaystackConfigurationFile(Assembly.GetExecutingAssembly());
            if (!File.Exists(configurationFile))
            {
                return;
            }

            string haystackDiagnosticsDirectory = FindHaystackDiagnosticsDirectory(configurationFile);
            if (haystackDiagnosticsDirectory != null)
            {
                AppDomain.CurrentDomain.AddAssemblyResolveDirectory(haystackDiagnosticsDirectory);
            }

            InitializeDiagnostics(configurationFile);
        }

        private static void InitializeDiagnostics(string configurationFile)
        {
            HaystackInitializer.LaunchDebuggerIfNecessary(configurationFile);
            HaystackInitializer.InitializeIfNecessary(configurationFile);
        }

        private static string FindHaystackDiagnosticsDirectory(string configurationFile)
        {
            using (TextReader reader = new StreamReader(configurationFile))
            {
                return XmlSerialization.Deserialize<BootstrapConfiguration>(reader).HaystackDiagnosticsDirectory;
            }
        }
    }
}
