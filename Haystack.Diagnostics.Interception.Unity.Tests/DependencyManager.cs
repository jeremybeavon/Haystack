using Haystack.Diagnostics.Interception.Tests;
using Microsoft.Practices.Unity;

namespace Haystack.Diagnostics.Interception.Unity.Tests
{
    public static class DependencyManager
    {
        private static readonly object simpleContainerLock = new object();
        private static IUnityContainer simpleContainer;

        public static IUnityContainer SimpleContainer
        {
            get
            {
                lock (simpleContainerLock)
                {
                    if (simpleContainer == null)
                    {
                        IUnityContainer container = new UnityContainer();
                        container.RegisterType<ISimpleService, SimpleService>();
                        simpleContainer = container;
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
