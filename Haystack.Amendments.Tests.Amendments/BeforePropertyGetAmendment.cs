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
            TestTrace.TraceText = string.Format("BeforePropertyGet(instance = {0}, propertyName = {1})", instance, propertyName);
        }
    }
}
