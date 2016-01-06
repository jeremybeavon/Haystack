using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.CodeCoverage;
using Haystack.Diagnostics.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Haystack.Diagnostics
{
    public sealed class HaystackRunner
    {
        private readonly IHaystackConfiguration configuration;
        private readonly string baseDirectory;

        public HaystackRunner(IHaystackConfiguration configuration)
            : this(configuration, Assembly.GetExecutingAssembly().AssemblyBaseDirectory())
        {
        }

        public HaystackRunner(IHaystackConfiguration configuration, string baseDirectory)
        {
            this.configuration = configuration;
            this.baseDirectory = baseDirectory;
        }

        public static void RunHaystackDiagnostics(IHaystackConfiguration configuration)
        {
            new HaystackRunner(configuration).RunHaystackDiagnostics();
        }
        
        private void RunHaystackDiagnostics()
        {
            InitializeAssemblyResolution();
            InitializeAmendments();
            ITestRunContext testRunContext = InitializeTestRunner();
            IEnumerable<CodeCoverageProvider> codeCoverageProviders = InitializeCodeCoverage(testRunContext);
            RunTests(testRunContext);
            FinishCodeCoverage(codeCoverageProviders);
        }

        private void InitializeAssemblyResolution()
        {
            List<string> referencedDirectories = new List<string>();
            referencedDirectories.Add(Path.Combine(baseDirectory, "References"));
            if (configuration.Interception != null)
            {
                foreach (IInterceptionConfiguration interception in configuration.Interception)
                {
                    referencedDirectories.Add(Path.Combine(baseDirectory, interception.InterceptionFramework, interception.InterceptionFrameworkVersion));
                }
            }

            if (configuration.StaticAnalysis != null)
            {
                foreach (IStaticAnalysisConfiguration staticAnalysis in configuration.StaticAnalysis)
                {
                    referencedDirectories.Add(Path.Combine(baseDirectory, staticAnalysis.StaticAnalysisFramework));
                }
            }

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assemblyFile = new AssemblyName(args.Name).Name + ".dll";
                string assemblyPath = referencedDirectories
                    .Select(referencedDirectory => Path.Combine(referencedDirectory, assemblyFile))
                    .FirstOrDefault(file => File.Exists(file));
                return assemblyPath == null ? null : Assembly.LoadFrom(assemblyPath);
            };
        }

        private void InitializeAmendments()
        {
            if (configuration.Amendments != null && configuration.Amendments.AssembliesToAmend != null)
            {
                foreach (string assemblyToAmend in configuration.Amendments.AssembliesToAmend)
                {
                    AmendmentSetupProvider.SetupIfNecessary(assemblyToAmend, configuration);
                }
            }
        }

        private ITestRunContext InitializeTestRunner()
        {
            IRunnerConfiguration runner = configuration.Runner;
            if (runner == null)
            {
                throw new InvalidOperationException("configuration.Runner cannot be null.");
            }

            return new TestRunContext()
            {
                Exe = Path.Combine(baseDirectory, runner.RunnerFramework, runner.RunnerFrameworkVersion, runner.RunnerExe),
                Arguments = runner.RunnerArguments
            };
        }

        private IEnumerable<CodeCoverageProvider> InitializeCodeCoverage(ITestRunContext testRunContext)
        {
            if (configuration.CodeCoverage == null)
            {
                return new CodeCoverageProvider[0];
            }

            List<CodeCoverageProvider> codeCoverageProviders = new List<CodeCoverageProvider>();
            string outputDirectory = configuration.OutputDirectory;
            foreach (ICodeCoverageConfiguration codeCoverage in configuration.CodeCoverage)
            {
                string assemblyPath = Path.Combine(baseDirectory, codeCoverage.CodeCoverageFramework, codeCoverage.CodeCoverageProviderAssembly);
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                CodeCoverageProviderAttribute attribute = assembly.GetCustomAttribute<CodeCoverageProviderAttribute>();
                if (attribute == null)
                {
                    throw new InvalidOperationException("Missing CodeCoverageProviderAttribute");
                }

                object codeCoverageProvider = Activator.CreateInstance(attribute.CodeCoverageProviderType);
                CodeCoverageContext codeCoverageContext = new CodeCoverageContext(codeCoverage.CodeCoverageFilter, outputDirectory);
                IBeforeTestRun beforeTestRun = codeCoverageProvider as IBeforeTestRun;
                if (beforeTestRun != null)
                {
                    beforeTestRun.BeforeTestRun(testRunContext, codeCoverageContext);
                }

                codeCoverageProviders.Add(new CodeCoverageProvider(codeCoverageProvider, codeCoverageContext));
            }

            return codeCoverageProviders;
        }

        private void RunTests(ITestRunContext testRunContext)
        {
            ProcessRunner.ExecuteProcess(new ProcessStartInfo(string.Format("\"{0}\" {1}", testRunContext.Exe, testRunContext.Arguments)));
        }

        private void FinishCodeCoverage(IEnumerable<CodeCoverageProvider> codeCoverageProviders)
        {
            foreach (CodeCoverageProvider provider in codeCoverageProviders)
            {
                IAfterTestRun afterTestRun = provider.Provider as IAfterTestRun;
                if (afterTestRun != null)
                {
                    afterTestRun.AfterTestRun(provider.Context);
                }
            }
        }
    }
}
