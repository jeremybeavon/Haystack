using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterMethodAmender : IMethodAmender
    {
        TReturnValue AfterMethod<TInstance, TReturnValue>(
            TInstance instance,
            MethodInfo method,
            object[] parameters,
            TReturnValue returnValue);
    }
}
