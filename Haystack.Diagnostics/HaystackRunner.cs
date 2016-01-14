using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.CodeCoverage;
using Haystack.Diagnostics.Configuration;
using Haystack.Core;
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

        private HaystackRunner(IHaystackConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public static void RunHaystackDiagnostics(IHaystackConfiguration configuration)
        {
            new HaystackRunner(configuration).RunHaystackDiagnostics();
        }
        
        private void RunHaystackDiagnostics()
        {
            InitializeAssemblyResolution();
            InitializeAmendments();
            IRunnerConfiguration runner = configuration.Runner;
            if (runner == null)
            {
                throw new InvalidOperationException("configuration.Runner cannot be null.");
            }

            string runnerDirectory = Path.Combine(
                configuration.HaystackBaseDirectory,
                "Runner",
                runner.RunnerFramework,
                runner.RunnerFrameworkVersion);
            InitializeTestFramework(runner, runnerDirectory);
            ITestRunContext testRunContext = InitializeTestRunner(runner, runnerDirectory);
            IEnumerable<CodeCoverageProvider> codeCoverageProviders = InitializeCodeCoverage(testRunContext);
            RunTests(testRunContext);
            FinishCodeCoverage(codeCoverageProviders);
        }

        private void InitializeAssemblyResolution()
        {
            List<string> referencedDirectories = new List<string>();
            foreach (IInterceptionConfiguration interception in configuration.Interception ?? new IInterceptionConfiguration[0])
            {
                string interceptionDirectory = Path.Combine(
                    configuration.HaystackBaseDirectory,
                    interception.InterceptionFramework,
                    interception.InterceptionFrameworkVersion);
                referencedDirectories.Add(interceptionDirectory);
            }

            foreach (IStaticAnalysisConfiguration staticAnalysis in configuration.StaticAnalysis ?? new IStaticAnalysisConfiguration[0])
            {
                referencedDirectories.Add(Path.Combine(configuration.HaystackBaseDirectory, staticAnalysis.StaticAnalysisFramework));
            }

            if (referencedDirectories.Count != 0)
            {
                AppDomain.CurrentDomain.AddAssemblyResolveDirectory(referencedDirectories.ToArray());
            }
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

        private void InitializeTestFramework(IRunnerConfiguration runner, string runnerDirectory)
        {
            string haystackAddinFile = string.IsNullOrWhiteSpace(runner.HaystackAddinName) ?
                string.Format("Haystack.Runner.{0}.dll", runner.RunnerFramework) :
                runner.HaystackAddinName;
            string haystackAddin = Path.Combine(runnerDirectory, "HaystackAddin", haystackAddinFile);
            if (File.Exists(haystackAddin))
            {
                AppDomain.CurrentDomain.AddAssemblyResolveDirectory(runnerDirectory);
                Assembly assembly = Assembly.LoadFrom(haystackAddin);
                RunnerFrameworkInitializerAttribute initializerAttribute = assembly.GetCustomAttribute<RunnerFrameworkInitializerAttribute>();
                if (initializerAttribute != null)
                {
                    Type initializerType = initializerAttribute.RunnerFrameworkInitializerType;
                    IRunnerFrameworkInitializer initializer = Activator.CreateInstance(initializerType) as IRunnerFrameworkInitializer;
                    if (initializer == null)
                    {
                        string message = string.Format(
                            "{0} does not implement IRunnerFrameworkInitializer",
                            initializerType.FullName);
                        throw new InvalidOperationException(message);
                    }

                    initializer.InitializeRunnerFramework(runner.AssemblyToTest);
                }
            } 
        }

        private ITestRunContext InitializeTestRunner(IRunnerConfiguration runner, string runnerDirectory)
        {
            string exe = Path.Combine(runnerDirectory, runner.RunnerExe);
            IRunnerArgumentsProvider runnerArgumentsProvider = runner.RunnerArgumentsProvider;
            return new TestRunContext()
            {
                Exe = exe,
                Arguments = runnerArgumentsProvider == null ? runner.RunnerArguments : runnerArgumentsProvider.BuildRunnerArguments()
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
                string assemblyPath = Path.Combine(
                    configuration.HaystackBaseDirectory,
                    codeCoverage.CodeCoverageFramework,
                    codeCoverage.CodeCoverageProviderAssembly);
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
            foreach (IRunnerInitializer runnerInitializer in configuration.Runner.RunnerInitializers ?? new IRunnerInitializer[0])
            {
                runnerInitializer.InitializeRunner(testRunContext);
            }

            string runnerCommand = string.Format("\"{0}\" {1}", testRunContext.Exe, testRunContext.Arguments);
            if (!string.IsNullOrWhiteSpace(testRunContext.StandardOutputFile))
            {
                runnerCommand = string.Format("{0} > \"{1}\"", runnerCommand, testRunContext.StandardOutputFile);
            }

            ProcessRunner.ExecuteProcess(new ProcessStartInfo(runnerCommand));
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
