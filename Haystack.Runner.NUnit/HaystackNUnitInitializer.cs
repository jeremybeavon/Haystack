using Haystack.Diagnostics;
using Haystack.Diagnostics.Amendments;
using Haystack.Runner.NUnit.Initializer;

[assembly: RunnerFrameworkInitializer(typeof(HaystackNUnitInitializer))]

namespace Haystack.Runner.NUnit.Initializer
{
    public sealed class HaystackNUnitInitializer : IRunnerFrameworkInitializer
    {
        public void InitializeRunnerFramework(string assemblyToTest)
        {
            AssemblyAmender.AddAssemblyAttribute(assemblyToTest, typeof(HaystackDiagnosticsAttribute));
        }
    }
}
