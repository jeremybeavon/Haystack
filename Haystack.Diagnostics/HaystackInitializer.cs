using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Configuration;
using Haystack.Diagnostics.TestIntegration;
using System.IO;
using VisualStudioDebuggerLauncher;

namespace Haystack.Diagnostics
{
    public static class HaystackInitializer
    {
        public const string LaunchDebuggerFileName = "launchdebugger";

        private static readonly object initializeLock = new object();
        private static bool isInitialized;
        private static IHaystackConfiguration configuration;

        public static IHaystackConfiguration InitializeIfNecessary(string configurationFile)
        {
            if (isInitialized || configurationFile == null)
            {
                return configuration;
            }

            lock (initializeLock)
            {
                if (isInitialized)
                {
                    return configuration;
                }

                Initialize(configurationFile);
                isInitialized = true;
                return configuration;
            }
        }

        public static void LaunchDebuggerIfNecessary(string configurationFile)
        {
            string launchDebgguerFile = Path.Combine(Path.GetDirectoryName(configurationFile), LaunchDebuggerFileName);
            if (File.Exists(launchDebgguerFile))
            {
                File.Delete(launchDebgguerFile);
                DebuggerLauncher.AttachDebugger("Haystack.sln");
            }
        }

        private static void Initialize(string configurationFile)
        {
            configuration = HaystackConfiguration.LoadFile(configurationFile);
            if (configuration.Amendments != null)
            {
                AmendmentRepository.Initialize(configuration.Amendments);
            }

            TestIntegrationRepository.Initialize(configuration.Runner);
            HaystackConfiguration.Current = configuration;
        }
    }
}
