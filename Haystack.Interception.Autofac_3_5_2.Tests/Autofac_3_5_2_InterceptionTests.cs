using System;
using Autofac;
using FluentAssertions;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Haystack.Interception.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Interception.Autofac.Tests
{
    [TestClass]
    public class Autofac_3_5_2_InterceptionTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            DependencyManager.DisposeDependencyManager();
            MethodCallTraceContext.Dispose();
        }

        [TestMethod]
        public void TestSimpleAutofacInterceptionWithContainerBuilder()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainerBuilder);
            DependencyManager.SimpleContainer.Resolve<ISimpleService>().TestMethod();
            MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
            trace.MethodCallThreads.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
            trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
        }

        [TestMethod]
        public void TestSimpleAutofacInterceptionWithContainer()
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
