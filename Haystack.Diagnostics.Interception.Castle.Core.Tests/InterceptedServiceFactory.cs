using System;

namespace Haystack.Diagnostics.Interception.Castle.Core.Tests
{
    public sealed class InterceptedServiceFactory : IServiceFactory
    {
        private readonly IServiceFactory serviceFactory;

        public InterceptedServiceFactory(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public T Resolve<T>(Func<T> constructor)
        {
            return (T)InstanceInterceptor.CreateInstance(typeof(T), serviceFactory.Resolve(constructor));
        }
    }
}
