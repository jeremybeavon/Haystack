using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class AfterVoidMethodAmendment : IAfterVoidMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }
        
        public void AfterMethod<TInstance>(TInstance instance, MethodInfo method, object[] parameters)
        {
            const string format = "AfterVoidMethod(instance = {0}, methodName = {1}, parameters = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, method.Name, string.Join(",", parameters));
        }
    }
}
