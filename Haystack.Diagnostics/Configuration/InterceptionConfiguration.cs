using Haystack.Core;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class InterceptionConfiguration : IInterceptionConfiguration
    {
        public string InterceptionFramework { get; set; }

        public string InterceptionFrameworkVersion { get; set; }

        [XmlArrayItem("InitializeType")]
        public List<string> InitializeInterception { get; set; }

        IEnumerable<IInitializeInterception> IInterceptionConfiguration.InitializeInterception
        {
            get { return TypeResolver.CreateInstances<IInitializeInterception>(InitializeInterception); }
        }
    }
}
