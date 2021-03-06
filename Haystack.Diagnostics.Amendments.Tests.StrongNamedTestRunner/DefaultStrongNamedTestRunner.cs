﻿using Haystack.Diagnostics.Amendments.Tests.Amendments;
using Haystack.Diagnostics.Amendments.Tests.StrongNamedTestTarget;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Amendments.Tests.StrongNamedTestRunner
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
