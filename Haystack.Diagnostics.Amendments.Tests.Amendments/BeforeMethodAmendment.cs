using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class BeforeMethodAmendment : IBeforeMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }

        public bool AmendMethod(Type type, string methodName, object[] parameters)
        {
            return true;
        }
        
        public void BeforeMethod<TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            const string format = "BeforeMethod(instance = {0}, methodName = {1}, parameters = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, methodName, string.Join(",", parameters));
        }
    }
}
