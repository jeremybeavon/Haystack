using System;
using Castle.Windsor;
using FluentAssertions;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Haystack.Diagnostics.Interception.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;

namespace Haystack.Diagnostics.Interception.Castle.Windsor.Tests
{
    [TestClass]
    public class CastleWindsor_3_3_0_InterceptionTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            MethodCallTraceContext.Dispose();
        }

        [TestMethod]
        public void TestSimpleCastleWindsor_3_3_0_InterceptionWithSetupAfterRegistration()
        {
            using (IWindsorContainer container = new WindsorContainer())
            {
                container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
                HaystackInterceptor.SetUp(container);
                container.Resolve<ISimpleService>().TestMethod();
                MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
                trace.MethodCallThreads.Count.Should().Be(1);
                trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
                trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
                trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
            }
        }

        [TestMethod]
        public void TestSimpleCastleWindsorInterceptionWithSetupBeforeRegistration()
        {
            using (IWindsorContainer container = new WindsorContainer())
            {
                HaystackInterceptor.SetUp(container);
                container.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
                container.Resolve<ISimpleService>().TestMethod();
                MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
                trace.MethodCallThreads.Count.Should().Be(1);
                trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
                trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
                trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
            }
        }
    }
}
