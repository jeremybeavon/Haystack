using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Haystack.Bootstrap
{
    [XmlRoot("HaystackConfiguration")]
    public sealed class BootstrapConfiguration
    {
        public string HaystackBaseDirectory { get; set; }
    }
}
