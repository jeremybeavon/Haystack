using Haystack.Core;
using System;
using System.IO;

namespace Haystack.Amendments
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
            string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
            if (File.Exists(Path.Combine(baseDirectory, "Haystack.Diagnostics.dll")))
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => args.ResolveDiagnosticsAssembly(baseDirectory);
            }
        }
    }
}
