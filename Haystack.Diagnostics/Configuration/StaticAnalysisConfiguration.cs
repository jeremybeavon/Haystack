using Haystack.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class StaticAnalysisConfiguration : IStaticAnalysisConfiguration
    {
        [Required]
        public string StaticAnalysisProvider { get; set; }

        [Required]
        public TypeConfiguration StaticAnalysisRunner { get; set; }

        public string Name { get; set; }

        [XmlArrayItem("Item")]
        public List<string> IncludedItems { get; set; }

        [XmlArrayItem("Item")]
        public List<string> ExcludedItems { get; set; }

        IStaticAnalysis IStaticAnalysisConfiguration.StaticAnalysisRunner
        {
            get { return TypeResolver.CreateInstance<IStaticAnalysis>(StaticAnalysisRunner); }
        }

        IEnumerable<string> IStaticAnalysisConfiguration.IncludedItems
        {
            get { return IncludedItems; }
        }

        IEnumerable<string> IStaticAnalysisConfiguration.ExcludedItems
        {
            get { return ExcludedItems; }
        }
    }
}
