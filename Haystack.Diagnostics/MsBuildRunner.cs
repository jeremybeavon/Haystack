using Microsoft.Build.BuildEngine;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Haystack.Diagnostics
{
    public static class MsBuildRunner
    {
        public static void RunMsBuildXml(string xml, IDictionary<string, string> properties)
        {
            using (TextReader reader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    ProjectRootElement rootElement = ProjectRootElement.Create(xmlReader);
                    ProjectCollection projectCollection = ProjectCollection.GlobalProjectCollection;
                    ProjectInstance project = new ProjectInstance(rootElement, properties, null, projectCollection);
                    BuildParameters parameters = new BuildParameters(new ProjectCollection())
                    {
                        Loggers = new ILogger[] { new ConsoleLogger() }
                    };
                    BuildResult result = BuildManager.DefaultBuildManager.Build(parameters, new BuildRequestData(project, new string[0]));
                    if (result.OverallResult == BuildResultCode.Failure)
                        throw new InvalidOperationException("Haystack diagnostics failed.");
                }
            }
        }
    }
}
