using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IPropertyAmender
    {
        bool AmendProperty(PropertyInfo property);

        bool AmendProperty(Type type, string propertyName);
    }
}
