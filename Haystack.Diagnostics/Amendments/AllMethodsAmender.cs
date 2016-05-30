using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public class AllMethodsAmender : IMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }

        public bool AmendMethod(Type type, string methodName, object[] parameters)
        {
            return true;
        }
    }
}
