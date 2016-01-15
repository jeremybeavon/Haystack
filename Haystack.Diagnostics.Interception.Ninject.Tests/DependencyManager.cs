using Ninject;
using Haystack.Diagnostics.Interception.Tests;

namespace Haystack.Diagnostics.Interception.Ninject.Tests
{
    public static class DependencyManager
    {
        private static readonly object simpleKernelLock = new object();
        private static IKernel simpleKernel;

        public static IKernel SimpleKernel
        {
            get
            {
                lock (simpleKernelLock)
                {
                    if (simpleKernel == null)
                    {
                        IKernel kernel = new StandardKernel();
                        kernel.Bind<ISimpleService>().To<SimpleService>();
                        simpleKernel = kernel;
                    }

                    return simpleKernel;
                }
            }
        }
        
        public static void DisposeDependencyManager()
        {
            lock (simpleKernelLock)
            {
                simpleKernel.Dispose();
                simpleKernel = null;
            }
        }
    }
}
