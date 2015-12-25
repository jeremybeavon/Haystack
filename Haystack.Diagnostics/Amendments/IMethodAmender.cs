using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IMethodAmender
    {
        bool AmendMethod(MethodInfo method);

        bool AmendMethod(Type type, string methodName, object[] parameters);
    }
}
