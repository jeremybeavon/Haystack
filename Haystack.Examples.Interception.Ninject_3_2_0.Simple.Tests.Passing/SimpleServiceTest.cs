using Ninject;
using NUnit.Framework;

namespace Haystack.Examples.Interception.Ninject.Simple.Tests
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
        public void TestSimpleService()
        {
            ISimpleService service = DependencyManager.SimpleKernel.Get<ISimpleService>();
            service.TestMethod1();
            service.TestMethod3();
            service.TestMethod2();
        }
    }
}
