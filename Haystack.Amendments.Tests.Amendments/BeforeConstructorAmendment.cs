using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Amendments.Tests.Amendments
{
    public sealed class BeforeConstructorAmendment : IBeforeConstructorAmender
    {
        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return true;
        }

        public bool AmendConstructor(Type type, object[] parameters)
        {
            return true;
        }

        public void BeforeConstructor<TInstance>(TInstance instance, object[] parameters)
        {
            const string format = "BeforeConstructor(instance = {0}, parameters = {1})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, string.Join(",", parameters));
        }
    }
}
