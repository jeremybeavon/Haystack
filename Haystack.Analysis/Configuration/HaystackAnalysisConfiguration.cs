using Haystack.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Haystack.Analysis.Configuration
{
    public sealed class HaystackAnalysisConfiguration : IHaystackAnalysisConfiguration
    {
        public HaystackAnalysisConfiguration()
        {
            CodeCoverage = new List<CodeCoverageConfiguration>();
            SourceControl = new List<SourceControlConfiguration>();
        }

        [Required]
        public string HaystackBaseDirectory { get; set; }

        public string HaystackAnalysisDirectory
        {
            get { return Path.Combine(HaystackBaseDirectory, "Analysis"); }
        }

        [Required]
        public string FailingTestOutputDirectory { get; set; }

        [Required]
        public string PassingTestOutputDirectory { get; set; }

        public string OutputDirectory { get; set; }

        [XmlElement("CodeCoverage")]
        public List<CodeCoverageConfiguration> CodeCoverage { get; set; }

        [XmlElement("SourceControl")]
        public List<SourceControlConfiguration> SourceControl { get; set; }

        IEnumerable<ICodeCoverageConfiguration> IHaystackAnalysisConfiguration.CodeCoverage
        {
            get { return CodeCoverage.Cast<ICodeCoverageConfiguration>(); }
        }

        IEnumerable<ISourceControlConfiguration> IHaystackAnalysisConfiguration.SourceControl
        {
            get { return SourceControl.Cast<ISourceControlConfiguration>(); }
        }

        public static IHaystackAnalysisConfiguration LoadFile(string fileName)
        {
            HaystackAnalysisConfiguration configuration = LoadText(File.ReadAllText(fileName));
            HaystackAnalysisConfigurationRelativePathResolver.ResolveRelativePaths(configuration, Path.GetDirectoryName(fileName));
            return configuration;
        }

        public static HaystackAnalysisConfiguration LoadText(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                HaystackAnalysisConfiguration configuration = XmlSerialization.Deserialize<HaystackAnalysisConfiguration>(reader);
                configuration.Validate();
                return configuration;
            }
        }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
