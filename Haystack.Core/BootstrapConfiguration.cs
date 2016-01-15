using System.IO;
using System.Xml.Serialization;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Core
#endif
{
    [XmlRoot("HaystackConfiguration")]
    public sealed class BootstrapConfiguration
    {
        public string HaystackDiagnosticsDirectory { get; set; }

        public static string GetHaystackBaseDirectory(string configurationFile)
        {
            using (TextReader reader = new StreamReader(configurationFile))
            {
                return XmlSerialization.Deserialize<BootstrapConfiguration>(reader).HaystackDiagnosticsDirectory;
            }
        }
    }
}
