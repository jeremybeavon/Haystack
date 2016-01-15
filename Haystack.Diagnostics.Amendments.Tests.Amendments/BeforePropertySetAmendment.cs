using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class BeforePropertySetAmendment : IBeforePropertySetAmender
    {
        public bool AmendProperty(PropertyInfo property)
        {
            return true;
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return true;
        }

        public TProperty BeforePropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            const string format = "BeforePropertySet(instance = {0}, propertyName = {1}, value = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, propertyName, value);
            return value;
        }
    }
}
