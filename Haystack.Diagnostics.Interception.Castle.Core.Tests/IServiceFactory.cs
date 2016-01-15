using System;

namespace Haystack.Diagnostics.Interception.Castle.Core.Tests
{
    public interface IServiceFactory
    {
        T Resolve<T>(Func<T> constructor);
    }
}
