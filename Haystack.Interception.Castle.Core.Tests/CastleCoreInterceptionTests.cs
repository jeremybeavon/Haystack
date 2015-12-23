using FluentAssertions;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Haystack.Interception.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Haystack.Interception.Castle.Core.Tests
{
    [TestClass]
    public class AutofacInterceptionTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            MethodCallTraceContext.Dispose();
        }

        [TestMethod]
        public void TestSimpleCastleCoreInterception()
        {
            ServiceFactory.Implementation = new InterceptedServiceFactory(new DefaultServiceFactory());
            ServiceFactory.Resolve<ISimpleService>(() => new SimpleService()).TestMethod();
            MethodCallTrace trace = MethodCallTraceContext.MethodCallTrace.BuildMethodCallTrace();
            trace.MethodCallThreads.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls.Count.Should().Be(1);
            trace.MethodCallThreads[0].MethodCalls[0].MethodName.Should().Be("TestMethod");
            trace.MethodCallThreads[0].MethodCalls[0].MethodCalls.Count.Should().Be(0);
        }
    }
}
