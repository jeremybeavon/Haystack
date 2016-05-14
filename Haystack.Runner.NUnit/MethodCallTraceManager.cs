using System;
using System.IO;
using System.Reflection;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;

namespace Haystack.Runner.NUnit
{
    public sealed class MethodCallTraceManager
    {
        private readonly string outputDirectory;
        private int currentTestIndex;
        private int currentFixtureSetUpIndex;

        public MethodCallTraceManager()
        {
            outputDirectory = HaystackConfiguration.Current.OutputDirectory;
        }

        public void SaveCallTrace(string testName)
        {
            currentTestIndex++;
            SaveCallTrace(string.Format("haystack.callTrace.{0}", currentTestIndex), testName);
        }

        public void SaveFixtureSetUpCallTrace()
        {
            string fileName = "haystack.callTrace.fixtureSetUp";
            string description = "FixtureSetUp";
            if (currentFixtureSetUpIndex != 0)
            {
                string suffix = "." + currentFixtureSetUpIndex;
                fileName += suffix;
                description += suffix;
            }

            SaveCallTrace(fileName, description);
            currentFixtureSetUpIndex++;
        }

        public void SaveFixtureTearDownCallTrace()
        {
            SaveCallTrace("haystack.callTrace.fixtureTearDown", "FixtureTearDown");
        }

        private static Type GetType(Assembly assembly, string typeName)
        {
            return assembly.GetType(typeName, true);
        }

        private void SaveCallTrace(string fileName, string description)
        {
            MethodCallTraceContext.Save(Path.Combine(outputDirectory, fileName), description);
        }
    }
}
