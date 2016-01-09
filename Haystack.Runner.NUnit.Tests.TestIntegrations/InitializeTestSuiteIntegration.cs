using Haystack.Diagnostics.TestIntegration;
using System.ComponentModel;
using System.Diagnostics;

namespace Haystack.Runner.NUnit.Tests.TestIntegrations
{
    [Description(TraceText)]
    public class InitializeTestSuiteIntegration : IInitializeTestSuite
    {
        public const string TraceText = "InitializeTestSuite called";

        public void InitializeTestSuite(string testSuiteName)
        {
            Trace.WriteLine(TraceText);
        }
    }
}
