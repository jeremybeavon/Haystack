using Haystack.Amendments.Tests.Amendments;
using Haystack.Amendments.Tests.TestTarget;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Amendments.Tests.TestRunner
{
    public static class DefaultTestRunner
    {
        public static string PropertyTest(string configurationText)
        {
            AmendmentRepository.Initialize(configurationText);
            new SimpleProperty("Instance1").TestProperty = new SimpleProperty("Instance2").TestProperty;
            return TestTrace.TraceText;
        }

        public static string HaystackPropertyTest(string configurationText)
        {
            AmendmentRepository.Initialize(configurationText);
            new SimpleProperty("Instance1").TestProperty = new SimpleProperty("Instance2").TestProperty;
            MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
            int threadCount = trace.MethodCallThreads.Count;
            int callCount = threadCount == 0 ? 0 : trace.MethodCallThreads[0].MethodCalls.Count;
            return string.Format("Threads: {0}, Calls: {1}", threadCount, callCount);
        }

        public static string ConstructorTest(string configurationText)
        {
            AmendmentRepository.Initialize(configurationText);
            new SimpleProperty("Instance1");
            return TestTrace.TraceText;
        }

        public static string VoidMethodTest(string configurationText)
        {
            AmendmentRepository.Initialize(configurationText);
            new SimpleVoidMethod().TestMethod();
            return TestTrace.TraceText;
        }

        public static string MethodTest(string configurationText)
        {
            AmendmentRepository.Initialize(configurationText);
            new SimpleMethod().TestMethod();
            return TestTrace.TraceText;
        }
    }
}
