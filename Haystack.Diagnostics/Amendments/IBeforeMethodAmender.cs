using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IBeforeMethodAmender : IMethodAmender
    {
        void BeforeMethod<TInstance>(TInstance instance, MethodInfo method, object[] parameters);
    }
}
