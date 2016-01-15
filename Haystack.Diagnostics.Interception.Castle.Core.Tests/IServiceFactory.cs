using System;

namespace Haystack.Interception.Castle.Core.Tests
{
    public interface IServiceFactory
    {
        T Resolve<T>(Func<T> constructor);
    }
}
