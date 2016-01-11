using Castle.Windsor;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple
{
    public static class DependencyManager
    {
        private static readonly object simpleContainerLock = new object();
        private static IWindsorContainer simpleContainer;
        
        public static IWindsorContainer SimpleContainer
        {
            get
            {
                lock (simpleContainerLock)
                {
                    if (simpleContainer == null)
                    {
                        simpleContainer = new WindsorContainer();
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
