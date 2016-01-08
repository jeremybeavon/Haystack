using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public class AllPropertiesAmender : IPropertyAmender
    {
        public bool AmendProperty(PropertyInfo property)
        {
            return true;
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return true;
        }
    }
}
