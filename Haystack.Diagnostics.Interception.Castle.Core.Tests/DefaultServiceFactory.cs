using System;

namespace Haystack.Diagnostics.Interception.Castle.Core.Tests
{
    public sealed class DefaultServiceFactory : IServiceFactory
    {
        public T Resolve<T>(Func<T> constructor)
        {
            return constructor();
        }
    }
}
