using StructureMap;
using Haystack.Interception.Tests;

namespace Haystack.Interception.StructureMap.Tests
{
    public static class DependencyManager
    {
        private static readonly object simpleContainerLock = new object();
        private static Container simpleContainer;

        public static IContainer SimpleContainer
        {
            get
            {
                lock (simpleContainerLock)
                {
                    if (simpleContainer == null)
                    {
                        simpleContainer = new Container(_ => _.For<ISimpleService>().Use<SimpleService>());
                    }

                    return simpleContainer;
                }
            }
        }
        
        public static void DisposeDependencyManager()
        {
            lock (simpleContainerLock)
            {
                simpleContainer.Dispose();
                simpleContainer = null;
            }
        }
    }
}
