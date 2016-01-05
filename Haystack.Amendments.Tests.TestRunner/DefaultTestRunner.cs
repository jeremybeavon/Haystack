using Haystack.Amendments.Tests.Amendments;
using Haystack.Amendments.Tests.TestTarget;

namespace Haystack.Amendments.Tests.TestRunner
{
    public static class DefaultTestRunner
    {
        public static string PropertyTest()
        {
            new SimpleProperty("Instance1").TestProperty = new SimpleProperty("Instance2").TestProperty;
            return TestTrace.TraceText;
        }
    }
}
