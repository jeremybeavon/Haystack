using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterVoidMethodAmender : IMethodAmender
    {
        void AfterMethod<TInstance>(TInstance instance, MethodInfo method, object[] parameters);
    }
}
