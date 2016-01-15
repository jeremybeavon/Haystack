using FluentAssertions;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Haystack.Interception.Tests;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Interception.Unity.Tests
{
    [TestClass]
    public class Unity_3_5_1304_InterceptionTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            DependencyManager.DisposeDependencyManager();
            MethodCallTraceContext.Dispose();
        }

        [TestMethod]
        public void TestSimpleUnity_3_5_1304_Interception()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
            DependencyManager.SimpleContainer.Resolve<ISimpleService>().TestMethod();
            MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
            trace.MethodCallThreads.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
            trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
        }
    }
}
