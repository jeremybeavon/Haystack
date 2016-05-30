using Haystack.Core;
using System;
using System.IO;

namespace Haystack.Diagnostics.Amendments
{
    public static class AmenderInitializer
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
            string haystackDiagnosticsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..");
            if (File.Exists(Path.Combine(haystackDiagnosticsDirectory, "Haystack.Diagnostics.dll")))
            {
                AppDomain.CurrentDomain.AddAssemblyResolveDirectory(haystackDiagnosticsDirectory);
            }
        }
    }
}
