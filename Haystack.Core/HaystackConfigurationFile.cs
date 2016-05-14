using System.IO;
using System.Reflection;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Core
#endif
{
    public static class HaystackConfigurationFile
    {
        public const string ConfigurationFileName = "haystack.config.xml";

        public static string GetHaystackConfigurationFile(Assembly assembly)
        {
            return Path.Combine(assembly.AssemblyBaseDirectory(), ConfigurationFileName);
        }
    }
}
