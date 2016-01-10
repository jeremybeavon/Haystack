﻿using Haystack.Diagnostics;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Runner.NUnit
{
    public sealed class HaystackNUnitInitializer : IRunnerFrameworkInitializer
    {
        public void InitializeRunnerFramework(string assemblyToTest)
        {
            AssemblyAmender.AddAssemblyAttribute(assemblyToTest, typeof(HaystackDiagnosticsAttribute));
        }
    }
}
