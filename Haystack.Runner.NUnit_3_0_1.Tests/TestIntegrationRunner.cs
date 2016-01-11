using Haystack.Runner.NUnit.TestRunner;

namespace Haystack.Runner.NUnit.Tests
{
    public sealed class TestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation> :
        AbstractNUnitTestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation>
        where TTestIntegrationImplementation : TTestIntegrationInterface
    {
        protected override string RunnerExe
        {
            get { return "nunit3-console.exe"; }
        }
        
        protected override string RunnerFrameworkVersion
        {
            get { return "3.0.1"; }
        }
    }
}
