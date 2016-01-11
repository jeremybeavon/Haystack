using Haystack.Runner.NUnit.TestRunner;

namespace Haystack.Runner.NUnit.Tests
{
    public sealed class TestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation> :
        AbstractNUnitTestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation>
        where TTestIntegrationImplementation : TTestIntegrationInterface
    {        
        protected override string RunnerExe
        {
            get { return "nunit-console-x86.exe"; }
        }

        protected override string RunnerFrameworkVersion
        {
            get { return "2.6.2"; }
        }
    }
}
