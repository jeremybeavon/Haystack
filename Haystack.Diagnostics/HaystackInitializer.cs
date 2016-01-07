using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Configuration;

namespace Haystack.Diagnostics
{
    public static class HaystackInitializer
    {
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

        private static void Initialize(string configurationFile)
        {
            configuration = HaystackConfiguration.LoadFile(configurationFile);
            AmendmentRepository.Initialize(configuration.Amendments);
        }
    }
}
