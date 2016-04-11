using System;

namespace Haystack.Diagnostics.Amendments
{
    public interface IConstructorAmender
    {
        bool AmendConstructor(Type type, object[] parameters);
    }
}
