using FluentAssertions;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Haystack.Interception.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Haystack.Interception.StructureMap.Tests
{
    [TestClass]
    public class AutofacInterceptionTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            DependencyManager.DisposeDependencyManager();
            MethodCallTraceContext.Dispose();
        }

        [TestMethod]
        public void TestSimpleStructureMap_4_0_1_318_Interception()
        {
            HaystackInterceptionPolicy.SetUp(DependencyManager.SimpleContainer);
            DependencyManager.SimpleContainer.GetInstance<ISimpleService>().TestMethod();
            MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
            trace.MethodCallThreads.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
            trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
        }
    }
}
