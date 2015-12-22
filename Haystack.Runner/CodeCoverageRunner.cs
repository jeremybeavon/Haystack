using System.Collections.Generic;
using Haystack.Diagnostics;
using Haystack.Runner.Properties;

namespace Haystack.Runner
{
    public sealed class CodeCoverageRunner
    {
        public string PathToOpenCover { get; set; }

        public string PathToReportGenerator { get; set; }

        public string PathToTestRunner { get; set; }

        public string TestRunnerArguments { get; set; }

        public string PathToCodeCoverageXmlFile { get; set; }

        public string PathToCodeCoverageReportDirectory { get; set; }

        public string CodeCoverageFilter { get; set; }
 
        public void RunCodeCoverage()
        {
            IDictionary<string, string> properties = new Dictionary<string, string>()
            {
                { "PathToOpenCover", PathToOpenCover },
                { "PathToReportGenerator", PathToReportGenerator },
                { "PathToTestRunner", PathToTestRunner },
                { "TestRunnerArguments", TestRunnerArguments },
                { "PathToCodeCoverageXmlFile", PathToCodeCoverageXmlFile },
                { "PathToCodeCoverageReportDirectory", PathToCodeCoverageReportDirectory },
                { "CodeCoverageFilter", CodeCoverageFilter }
            };
            MsBuildRunner.RunMsBuildXml(Resources.CodeCoverage, properties);
        }
    }
}
