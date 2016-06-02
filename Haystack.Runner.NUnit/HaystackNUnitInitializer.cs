using Haystack.Core;
using Haystack.Diagnostics;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Configuration;
using Haystack.Runner.NUnit.Initializer;
using System.IO;
using System.Reflection;

[assembly: RunnerFrameworkInitializer(typeof(HaystackNUnitInitializer))]

namespace Haystack.Runner.NUnit.Initializer
{
    public sealed class HaystackNUnitInitializer : IRunnerFrameworkInitializer
    {
        public void InitializeRunnerFramework(IHaystackConfiguration haystackConfiguration)
        {
            string assemblyToTest = haystackConfiguration.Runner.AssemblyToTest;
            AssemblyAmender.AddAssemblyAttribute(assemblyToTest, typeof(HaystackDiagnosticsAttribute));
            string assemblyDirectory = Path.GetDirectoryName(assemblyToTest);
            if (!File.Exists(Path.Combine(assemblyToTest, "Haystack.Bootstrap.dll")))
            {
                string haystackBaseDirectory = haystackConfiguration.HaystackBaseDirectory;
                string bootstrapDirectory = Path.Combine(haystackBaseDirectory, "Bootstrap", FrameworkVersion.Current);
                foreach (string file in Directory.GetFiles(bootstrapDirectory, "Haystack.Bootstrap.*"))
                {
                    File.Copy(file, Path.Combine(assemblyDirectory, Path.GetFileName(file)), true);
                }
            }
            
            if (!File.Exists(Path.Combine(assemblyDirectory, "Haystack.Runner.NUnit.dll")))
            {
                string baseDirectory = Assembly.GetExecutingAssembly().AssemblyBaseDirectory();
                string runnerAddinDirectory = Path.Combine(baseDirectory, @"..\HaystackAddin");
                foreach (string file in Directory.GetFiles(runnerAddinDirectory, "Haystack.Runner.NUnit.*"))
                {
                    File.Copy(file, Path.Combine(assemblyDirectory, Path.GetFileName(file)), true);
                }
            }
        }
    }
}
