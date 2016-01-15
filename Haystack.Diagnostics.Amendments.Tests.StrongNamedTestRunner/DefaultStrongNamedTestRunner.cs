using Haystack.Amendments.Tests.Amendments;
using Haystack.Amendments.Tests.StrongNamedTestTarget;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Amendments.Tests.StrongNamedTestRunner
{
    public static class DefaultStrongNamedTestRunner
    {
        public static string PropertyTest(string configurationText)
        {
            AmendmentRepository.Initialize(configurationText);
            new SimpleProperty("Instance1").TestProperty = new SimpleProperty("Instance2").TestProperty;
            return TestTrace.TraceText;
        }
    }
}
