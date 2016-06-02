using AppDomainCallbackExtensions;
using Haystack.Diagnostics.Amendments.Tests.Amendments;
using Haystack.Core;
using Haystack.Core.IO;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Configuration;
using System;
using System.IO;
using System.Linq;
using TextSerialization;

namespace Haystack.Diagnostics.Amendments.Tests
{
    public class AmendmentTestRunner
    {
        private readonly string baseDirectory;

        public AmendmentTestRunner()
        {
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string testTargetDirectory = Path.Combine(baseDirectory, "TestTarget");
            TargetDll = Path.Combine(testTargetDirectory, "Haystack.Diagnostics.Amendments.Tests.TestTarget.dll");
            TestRunnerDll = Path.Combine(testTargetDirectory, "Haystack.Diagnostics.Amendments.Tests.TestRunner.dll");
        }

        public string TestName { get; set; }
        
        public HaystackConfiguration Configuration { get; set; }

        public Func<string, string> TestRunnerMethod { get; set; }
        
        protected string BaseDirectory
        {
            get { return baseDirectory; }
        }

        protected string TargetDll { get; set; }

        protected string TestRunnerDll { get; set; }

        protected string StrongNameKey { get; set; }

        public string RunTest()
        {
            Configuration.OutputDirectory = baseDirectory;
            Configuration.HaystackBaseDirectory = baseDirectory;
            Configuration.Amendments.StrongNameKeyFile = StrongNameKey;
            string testDirectory = Path.Combine(baseDirectory, TestName);
            SetUpTestRun(testDirectory);
            AppDomainSetup appDomainSetup = new AppDomainSetup()
            {
                ApplicationBase = testDirectory
            };
            using (DisposableAppDomain appDomain = new DisposableAppDomain("TestRunner", appDomainSetup))
            {
                return appDomain.AppDomain.DoCallBackWithResponse<TestCallback, DataContractSerialization, string>(
                    new TestCallback(Path.Combine(testDirectory, Path.GetFileName(TestRunnerDll)), TestRunnerMethod, Configuration),
                    new DataContractSerialization());
            }
        }
        
        private void SetUpTestRun(string testDirectory)
        {
            DirectoryCopy.CopyDirectory(Path.GetDirectoryName(TargetDll), testDirectory);
            UpdateConfiguration(testDirectory);
            string assemblyName = Path.GetFileNameWithoutExtension(TargetDll);
            (new AmendmentSetupProvider(Path.Combine(testDirectory, Path.GetFileName(TargetDll)), Configuration)
            {
                AfterthoughtAmenderExe = Path.Combine(baseDirectory, AmendmentSetupProvider.AfterthoughtAmenderExeFileName),
                AmendmentsDll = Path.Combine(baseDirectory, AmendmentSetupProvider.AmendmentsDllFileName)
            }).SetupIfNecessary();
        }

        private void UpdateConfiguration(string testDirectory)
        {
            AmendmentConfiguration configuration = Configuration.Amendments;
            foreach (TypeConfiguration type in configuration.AfterConstructorAmendments.Concat(
                configuration.AfterMethodAmendments).Concat(
                configuration.AfterPropertyGetAmendments).Concat(
                configuration.AfterPropertySetAmendments).Concat(
                configuration.AfterVoidMethodAmendments).Concat(
                configuration.BeforeConstructorAmendments).Concat(
                configuration.BeforeMethodAmendments).Concat(
                configuration.BeforePropertyGetAmendments).Concat(
                configuration.BeforePropertySetAmendments).Concat(
                configuration.CatchConstructorAmendments).Concat(
                configuration.CatchMethodAmendments).Concat(
                configuration.CatchVoidMethodAmendments).Concat(
                configuration.FinallyMethodAmendments).Concat(
                configuration.HaystackConstructorAmendments).Concat(
                configuration.HaystackMethodAmendments).Concat(
                configuration.HaystackPropertyAmendments))
            {
                type.AssemblyFile = Path.Combine(testDirectory, Path.GetFileName(type.AssemblyFile));
            }
        }
    }
}
