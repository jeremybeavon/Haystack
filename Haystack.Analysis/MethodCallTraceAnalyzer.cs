using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using Haystack.Diagnostics.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Haystack.Analysis
{
    public sealed class MethodCallTraceAnalyzer : IHaystackAnalyzer
    {
        public void Analyze(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
        {
            string[] passingCallTraceFiles = GetCallTraceFiles(passingConfiguration);
            List<string> failingCallTraceFiles = GetCallTraceFiles(failingConfiguration).ToList();
            string failingOutputDirectory = failingConfiguration.OutputDirectory;
            foreach (string passingCallTraceFile in passingCallTraceFiles)
            {
                string failingCallTraceFile = Path.Combine(failingOutputDirectory, Path.GetFileName(passingCallTraceFile));
                if (failingCallTraceFiles.Remove(failingCallTraceFile))
                {
                    AnalyzeCallTraceFiles(passingCallTraceFile, failingCallTraceFile);
                }
                else
                {

                }
            }
        }

        private static void CompareObjectInstances()
        {
            //IReadOnlyDictionary<ObjectType, IReadOnlyList<ObjectInstance>> standardizedObjectInstances

        }

        private static void AnalyzeCallTraceFiles(string passingCallTraceFile, string failingCallTraceFile)
        {
            MethodCallTrace passingCallTrace = new MethodCallTraceProvider().Load(passingCallTraceFile);
            MethodCallTrace failingCallTrace = new MethodCallTraceProvider().Load(failingCallTraceFile);

        }

        private static string[] GetCallTraceFiles(IHaystackConfiguration configuration)
        {
            return Directory.GetFiles(configuration.OutputDirectory, "haystack.callTrace.*");
        }
    }
}
