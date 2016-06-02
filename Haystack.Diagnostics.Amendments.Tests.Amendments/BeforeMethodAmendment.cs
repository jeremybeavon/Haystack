using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class BeforeMethodAmendment : IBeforeMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }
        
        public void BeforeMethod<TInstance>(TInstance instance, MethodInfo method, object[] parameters)
        {
            const string format = "BeforeMethod(instance = {0}, methodName = {1}, parameters = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, method.Name, string.Join(",", parameters));
        }
    }
}
