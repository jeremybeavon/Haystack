using System;

namespace Haystack.Interception.Castle.Core.Tests
{
    public static class ServiceFactory
    {
        private static readonly object implementationLock = new object();
        private static IServiceFactory implementation;

        public static IServiceFactory Implementation
        {
            get
            {
                lock (implementationLock)
                {
                    if (implementation == null)
                    {
                        implementation = new DefaultServiceFactory();
                    }

                    return implementation;
                }
            }
            set
            {
                lock (implementationLock)
                {
                    implementation = value;
                }
            } 
        }

        public static T Resolve<T>(Func<T> constructor)
        {
            return Implementation.Resolve(constructor);
        }
    }
}
