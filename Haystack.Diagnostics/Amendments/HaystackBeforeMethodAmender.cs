using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class HaystackBeforeMethodAmender : HaystackMethodAmender, IBeforeMethodAmender
    {
        public HaystackBeforeMethodAmender(IMethodAmender amender)
            : base(amender)
        {
        }

        public void BeforeMethod<TInstance>(TInstance instance, MethodInfo method, object[] parameters)
        {
            MethodCallTraceContext.MethodCallTrace.EnterMethodCall(instance, method, parameters);
        }
    }
}
