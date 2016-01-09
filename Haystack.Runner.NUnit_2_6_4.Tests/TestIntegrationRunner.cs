using Haystack.Core;
using Haystack.Runner.TestRunner;
using System;
using System.IO;
using System.Reflection;

namespace Haystack.Runner.NUnit.Tests
{
    public sealed class TestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation> :
        AbstractTestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation>
        where TTestIntegrationImplementation : TTestIntegrationInterface
    {
        protected override string AssemblyToTest
        {
            get { return Assembly.GetExecutingAssembly().AssemblyFilePath(); }
        }

        protected override string HaystackBaseDirectory
        {
            get { return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Haystack")); }
        }
        
        protected override string RunnerExe
        {
            get { return "nunit-console-x86.exe"; }
        }

        protected override string RunnerFramework
        {
            get { return "NUnit"; }
        }

        protected override string RunnerFrameworkVersion
        {
            get { return "2.6.4"; }
        }
    }
}
