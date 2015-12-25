using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IHaystackAmender
    {
        bool AmendProperty(Type type, string propertyName);

        bool AmendConstructor(ConstructorInfo constructor);

        bool AmendConstructor(Type type, object[] parameters);

        /*bool AmendMethod(MethodInfo method);

        */
    }
}
