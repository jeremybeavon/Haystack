using Haystack.Diagnostics.TestIntegration;
using System.ComponentModel;
using System.Diagnostics;

namespace Haystack.Runner.NUnit.Tests.TestIntegrations
{
    [Description(TraceText)]
    public class CleanUpTestMethodIntegration : ICleanUpTestMethod
    {
        public const string TraceText = "CleanUpTestMethod called";

        public void CleanUpTestMethod(string testMethodName)
        {
            Trace.WriteLine(TraceText);
        }
    }
}
