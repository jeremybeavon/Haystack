using System;
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
        public const string DefaultConfigurationFileName = "haystack.config.xml";

        public HaystackConfiguration()
        {
            CodeCoverage = new List<CodeCoverageConfiguration>();
            Interception = new List<InterceptionConfiguration>();
            StaticAnalysis = new List<StaticAnalysisConfiguration>();
            SourceControl = new List<SourceControlConfiguration>();
        }

        public string OutputDirectory { get; set; }

        public AmendmentConfiguration Amendments { get; set; }

        public List<CodeCoverageConfiguration> CodeCoverage { get; set; }

        public List<InterceptionConfiguration> Interception { get; set; }

        public List<StaticAnalysisConfiguration> StaticAnalysis { get; set; }

        public List<SourceControlConfiguration> SourceControl { get; set; }

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

        public static HaystackConfiguration LoadDefaultFile()
        {
            return LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigurationFileName));
        }

        public static HaystackConfiguration LoadFile(string fileName)
        {
            return LoadText(File.ReadAllText(fileName));
        }

        public static HaystackConfiguration LoadText(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                return (HaystackConfiguration)new XmlSerializer(typeof(HaystackConfiguration)).Deserialize(reader);
            }
        }
    }
}
