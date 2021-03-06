﻿using FluentAssertions;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Haystack.Diagnostics.Interception.Tests;
using Ninject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Diagnostics.Interception.Ninject.Tests
{
    [TestClass]
    public class Ninject_3_2_0_InterceptionTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            DependencyManager.DisposeDependencyManager();
            MethodCallTraceContext.Dispose();
        }

        [TestMethod]
        public void TestSimpleNinject_3_2_0_Interception()
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleKernel);
            DependencyManager.SimpleKernel.Get<ISimpleService>().TestMethod();
            MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
            trace.MethodCallThreads.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
            trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
        }
    }
}
