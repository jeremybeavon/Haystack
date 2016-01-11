using FluentAssertions;
using Haystack.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.StaticAnalysis.EnvironmentVariables.Tests
{
    [TestClass]
    public sealed class EnvironmentVariablesStaticAnalysisTests
    {
        [TestMethod]
        public void TestEnvironmentVariablesStaticAnalysis()
        {
            const string dummyVariableName = "Test";
            Environment.SetEnvironmentVariable(dummyVariableName, "Test1", EnvironmentVariableTarget.Process);
            EnvironmentVariablesStaticAnalysis staticAnalysis = new EnvironmentVariablesStaticAnalysis();
            IEnumerable<string> includedItems = new string[] { dummyVariableName };
            StaticAnalysisOutput staticAnalysisOutput = staticAnalysis.RunInitialAnalysis(includedItems, null);
            Environment.SetEnvironmentVariable(dummyVariableName, "Test2", EnvironmentVariableTarget.Process);
            staticAnalysis.RunFinalAnalysis(staticAnalysisOutput, includedItems, null);
            staticAnalysisOutput.Before.Count.Should().Be(1);
            staticAnalysisOutput.Before[0].Should().Be("Test = Test1");
            staticAnalysisOutput.After.Count.Should().Be(1);
            staticAnalysisOutput.After[0].Should().Be("Test = Test2");
        }
    }
}
