using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Haystack.Bootstrap
{
    public static class HaystackInitializer
    {
        private const string ConfigurationFileName = "haystack.config.xml";
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

                isInitialized = true;
                string haystackBaseDirectory = FindHaystackBaseDirectory();
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {

                }
            }
        }

        private static void Initialize()
        {

        }

        private static string FindHaystackBaseDirectory()
        {
            string configurationFile = HaystackConfigurationFile.GetHaystackConfigurationFile(Assembly.GetExecutingAssembly());
            using (TextReader reader = new StreamReader(configurationFile))
            {
                return XmlSerialization.Deserialize<BootstrapConfiguration>(reader).HaystackBaseDirectory;
            }
        }
    }
}
