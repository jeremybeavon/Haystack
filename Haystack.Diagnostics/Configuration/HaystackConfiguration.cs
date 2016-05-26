using Haystack.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class HaystackConfiguration : IHaystackConfiguration
    {
        public HaystackConfiguration()
        {
            Amendments = new AmendmentConfiguration();
            CodeCoverage = new List<CodeCoverageConfiguration>();
            Interception = new List<InterceptionConfiguration>();
            Runner = new RunnerConfiguration();
            StaticAnalysis = new List<StaticAnalysisConfiguration>();
        }

        public static IHaystackConfiguration Current { get; internal set; }

        [Required]
        public string HaystackBaseDirectory { get; set; }

        public string HaystackRunnerDirectory
        {
            get { return Path.Combine(HaystackBaseDirectory, "Runner", FrameworkVersion.Current); }
        }

        public string HaystackDiagnosticsDirectory
        {
            get { return Path.Combine(HaystackRunnerDirectory, "Diagnostics"); }
        }

        [Required]
        public string OutputDirectory { get; set; }

        public AmendmentConfiguration Amendments { get; set; }

        [XmlElement("CodeCoverage")]
        public List<CodeCoverageConfiguration> CodeCoverage { get; set; }

        [XmlElement("Interception")]
        public List<InterceptionConfiguration> Interception { get; set; }
        
        [XmlElement("StaticAnalysis")]
        public List<StaticAnalysisConfiguration> StaticAnalysis { get; set; }

        [Required]
        public RunnerConfiguration Runner { get; set; }

        IAmendmentConfiguration IHaystackConfiguration.Amendments
        {
            get { return Amendments; }
        }

        IEnumerable<ICodeCoverageConfiguration> IHaystackConfiguration.CodeCoverage
        {
            get { return CodeCoverage; }
        }

        IEnumerable<IInterceptionConfiguration> IHaystackConfiguration.Interception
        {
            get { return Interception.Cast<IInterceptionConfiguration>(); }
        }

        IEnumerable<IStaticAnalysisConfiguration> IHaystackConfiguration.StaticAnalysis
        {
            get { return StaticAnalysis.Cast<IStaticAnalysisConfiguration>(); }
        }
        
        IRunnerConfiguration IHaystackConfiguration.Runner
        {
            get { return Runner; }
        }
        
        public static IHaystackConfiguration LoadFile(string fileName)
        {
            HaystackConfiguration configuration = LoadText(File.ReadAllText(fileName));
            HaystackConfigurationRelativePathResolver.ResolveRelativePaths(configuration, Path.GetDirectoryName(fileName));
            configuration.Initialize();
            return configuration;
        }

        public static HaystackConfiguration LoadText(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                HaystackConfiguration configuration = XmlSerialization.Deserialize<HaystackConfiguration>(reader);
                configuration.Validate();
                return configuration;
            }
        }
        
        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HaystackConfiguration));
            StringBuilder textBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true
            };
            using (XmlWriter xmlWriter = XmlWriter.Create(textBuilder, settings))
            {
                serializer.Serialize(xmlWriter, this);
            }

            return textBuilder.ToString();
        }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }

        public void Initialize()
        {
            Directory.CreateDirectory(OutputDirectory);
        }
    }
}
