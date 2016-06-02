using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class AfterMethodAmendment : IAfterMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }
        
        public TReturnValue AfterMethod<TInstance, TReturnValue>(TInstance instance, MethodInfo method, object[] parameters, TReturnValue returnValue)
        {
            const string format = "AfterMethod(instance = {0}, methodName = {1}, parameters = {2}, returnValue = {3})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, method.Name, string.Join(",", parameters), returnValue);
            return returnValue;
        }
    }
}
