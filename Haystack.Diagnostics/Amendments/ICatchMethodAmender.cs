using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface ICatchMethodAmender : IMethodAmender
    {
        TReturnValue CatchMethod<TInstance, TException, TReturnValue>(
            TInstance instance,
            MethodInfo method,
            TException exception,
            object[] parameters);
    }
}
