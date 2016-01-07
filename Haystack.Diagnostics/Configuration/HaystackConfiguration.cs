using Haystack.Core;
using System.Collections.Generic;
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
            CodeCoverage = new List<CodeCoverageConfiguration>();
            Interception = new List<InterceptionConfiguration>();
            SourceControl = new List<SourceControlConfiguration>();
            StaticAnalysis = new List<StaticAnalysisConfiguration>();
        }

        public string HaystackBaseDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public AmendmentConfiguration Amendments { get; set; }

        public List<CodeCoverageConfiguration> CodeCoverage { get; set; }

        public List<InterceptionConfiguration> Interception { get; set; }

        public List<SourceControlConfiguration> SourceControl { get; set; }

        public List<StaticAnalysisConfiguration> StaticAnalysis { get; set; }

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

        IEnumerable<ISourceControlConfiguration> IHaystackConfiguration.SourceControl
        {
            get { return SourceControl.Cast<ISourceControlConfiguration>(); }
        }

        IRunnerConfiguration IHaystackConfiguration.Runner
        {
            get { return Runner; }
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
        
        public static HaystackConfiguration LoadFile(string fileName)
        {
            return LoadText(File.ReadAllText(fileName));
        }

        public static HaystackConfiguration LoadText(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                return XmlSerialization.Deserialize<HaystackConfiguration>(reader);
            }
        }
    }
}
