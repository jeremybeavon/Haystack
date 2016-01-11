using Autofac;
using NUnit.Framework;

namespace Haystack.Examples.Interception.Autofac.Simple.Tests
{
    [TestFixture]
    public sealed class SimpleServiceTest
    {
        [TearDown]
        public void CleanUp()
        {
            DependencyManager.DisposeDependencyManager();
        }

        [Test]
        public void TestSimpleServiceWithContainerBuilder()
        {
            ISimpleService service = DependencyManager.SimpleContainerBuilder.Build().Resolve<ISimpleService>();
            service.TestMethod1();
            service.TestMethod3();
            service.TestMethod2();
        }

        [Test]
        public void TestSimpleServiceWithContainer()
        {
            ISimpleService service = DependencyManager.SimpleContainer.Resolve<ISimpleService>();
            service.TestMethod1();
            service.TestMethod3();
            service.TestMethod2();
        }
    }
}
