using System;
using System.IO;
using System.Reflection;
using Haystack.Diagnostics;

namespace Haystack.Runner.NUnit
{
    public sealed class MethodCallTraceManager
    {
        private readonly string baseDirectory;
        private int currentTestIndex;
        private int currentFixtureSetUpIndex;

        public MethodCallTraceManager()
        {
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
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
            MethodCallTraceContext.Save(Path.Combine(baseDirectory, fileName), description);
        }
    }
}
