using Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple
{
    public static class DependencyManager
    {
        private static readonly object simpleContainerBuilderLock = new object();
        private static readonly object simpleContainerLock = new object();
        private static ContainerBuilder simpleContainerBuilder;
        private static IContainer simpleContainer;

        public static ContainerBuilder SimpleContainerBuilder
        {
            get
            {
                lock (simpleContainerBuilderLock)
                {
                    if (simpleContainerBuilder == null)
                    {
                        ContainerBuilder containerBuilder = new ContainerBuilder();
                        containerBuilder.RegisterType<SimpleService>().As<ISimpleService>();
                        simpleContainerBuilder = containerBuilder;
                    }

                    return simpleContainerBuilder;
                }
            }
        }

        public static IContainer SimpleContainer
        {
            get
            {
                lock (simpleContainerLock)
                {
                    if (simpleContainer == null)
                    {
                        simpleContainer = SimpleContainerBuilder.Build();
                    }

                    return simpleContainer;
                }
            }
        }

        public static void DisposeDependencyManager()
        {
            lock (simpleContainerBuilderLock)
            {
                simpleContainerBuilder = null;
            }

            lock (simpleContainerLock)
            {
                simpleContainer.Dispose();
                simpleContainer = null;
            }
        }
    }
}
