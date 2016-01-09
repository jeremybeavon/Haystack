using Haystack.Diagnostics.TestIntegration;
using System.ComponentModel;
using System.Diagnostics;

namespace Haystack.Runner.NUnit.Tests.TestIntegrations
{
    [Description(TraceText)]
    public class CleanUpTestSuiteIntegration : ICleanUpTestSuite
    {
        public const string TraceText = "CleanUpTestSuite called";

        public void CleanUpTestSuite(string testSuiteName)
        {
            Trace.WriteLine(TraceText);
        }
    }
}
