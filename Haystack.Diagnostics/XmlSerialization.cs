using System.IO;
using System.Xml.Serialization;

#if HAYSTACK_BOOTSTRAP
namespace Haystack.Bootstrap
#else
namespace Haystack.Diagnostics
#endif
{
    public static class XmlSerialization
    {
        public static T Deserialize<T>(TextReader reader)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }
    }
}
