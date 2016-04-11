using System;

namespace Haystack.Diagnostics.Amendments
{
    public interface IMethodAmender
    {
        bool AmendMethod(Type type, string methodName, object[] parameters);
    }
}
