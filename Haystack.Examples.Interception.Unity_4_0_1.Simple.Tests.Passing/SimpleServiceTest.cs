using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Haystack.Examples.Interception.Unity.Simple.Tests
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
            ISimpleService service = DependencyManager.SimpleContainer.Resolve<ISimpleService>();
            service.TestMethod1();
            service.TestMethod3();
            service.TestMethod2();
        }
    }
}
