using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface ICatchVoidMethodAmender : IMethodAmender
    {
        void CatchMethod<TInstance, TException>(
            TInstance instance,
            MethodInfo method,
            TException exception,
            object[] parameters);
    }
}
