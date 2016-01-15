using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class AfterConstructorAmendment : IAfterConstructorAmender
    {
        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return true;
        }

        public bool AmendConstructor(Type type, object[] parameters)
        {
            return true;
        }
        
        public void AfterConstructor<TInstance>(TInstance instance, object[] parameters)
        {
            const string format = "AfterConstructor(instance = {0}, parameters = {1})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, string.Join(",", parameters));
        }
    }
}
