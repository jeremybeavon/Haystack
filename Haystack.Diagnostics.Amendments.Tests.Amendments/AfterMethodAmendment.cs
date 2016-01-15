using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Amendments.Tests.Amendments
{
    public sealed class AfterMethodAmendment : IAfterMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }

        public bool AmendMethod(Type type, string methodName, object[] parameters)
        {
            return true;
        }
        
        public TReturnValue AfterMethod<TInstance, TReturnValue>(TInstance instance, string methodName, object[] parameters, TReturnValue returnValue)
        {
            const string format = "AfterMethod(instance = {0}, methodName = {1}, parameters = {2}, returnValue = {3})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, methodName, string.Join(",", parameters), returnValue);
            return returnValue;
        }
    }
}
