﻿using System.IO;
using System.Reflection;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Diagnostics.Configuration
#endif
{
    public static class HaystackConfigurationFile
    {
        public const string DefaultConfigurationFileName = "haystack.config.xml";

        public static string GetHaystackConfigurationFile(Assembly assembly)
        {
            return Path.Combine(assembly.AssemblyBaseDirectory(), DefaultConfigurationFileName);
        }
    }
}
