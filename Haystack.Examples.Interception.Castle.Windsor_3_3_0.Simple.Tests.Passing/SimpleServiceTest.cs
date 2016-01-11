using Castle.MicroKernel.Registration;
using NUnit.Framework;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple.Tests
{
    [TestFixture]
    public sealed class SimpleServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            DependencyManager.SimpleContainer.Register(Component.For<ISimpleService>().ImplementedBy<SimpleService>());
        }

        [TearDown]
        public void CleanUp()
        {
            DependencyManager.DisposeDependencyManager();
        }

        [Test]
        public void TestSimpleService()
        {
            ISimpleService service = DependencyManager.SimpleContainer.Resolve<ISimpleService>();
            service.TestMethod1();
            service.TestMethod3();
            service.TestMethod2();
        }
    }
}
