using Haystack.Diagnostics.TestIntegration;
using System.ComponentModel;
using System.Diagnostics;

namespace Haystack.Runner.NUnit.Tests.TestIntegrations
{
    [Description(TraceText)]
    public sealed class InitializeTestFrameworkIntegration : IInitializeTestFramework
    {
        public const string TraceText = "InitializeTestFramework called";

        public void InitializeTestFramework()
        {
            Trace.WriteLine(TraceText);
        }
    }
}
