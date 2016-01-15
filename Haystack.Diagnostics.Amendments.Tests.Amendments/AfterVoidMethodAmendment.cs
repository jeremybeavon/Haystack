using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class AfterVoidMethodAmendment : IAfterVoidMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }

        public bool AmendMethod(Type type, string methodName, object[] parameters)
        {
            return true;
        }

        public void AfterMethod<TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            const string format = "AfterVoidMethod(instance = {0}, methodName = {1}, parameters = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, methodName, string.Join(",", parameters));
        }
    }
}
