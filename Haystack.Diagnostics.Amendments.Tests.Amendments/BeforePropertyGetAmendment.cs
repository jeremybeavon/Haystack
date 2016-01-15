using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Amendments.Tests.Amendments
{
    public sealed class BeforePropertyGetAmendment : IBeforePropertyGetAmender
    {
        public bool AmendProperty(PropertyInfo property)
        {
            return true;
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return true;
        }

        public void BeforePropertyGet<TInstance>(TInstance instance, string propertyName)
        {
            const string format = "BeforePropertyGet(instance = {0}, propertyName = {1})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, propertyName);
        }
    }
}
