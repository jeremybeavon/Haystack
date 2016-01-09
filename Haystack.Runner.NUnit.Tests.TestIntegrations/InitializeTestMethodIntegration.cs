using Haystack.Diagnostics.TestIntegration;
using System.ComponentModel;
using System.Diagnostics;

namespace Haystack.Runner.NUnit.Tests.TestIntegrations
{
    [Description(TraceText)]
    public class InitializeTestMethodIntegration : IInitializeTestMethod
    {
        public const string TraceText = "InitializeTestMethod called";

        public void InitializeTestMethod(string testMethodName)
        {
            Trace.WriteLine(TraceText);
        }
    }
}
