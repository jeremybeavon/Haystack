using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IFinallyMethodAmender : IMethodAmender
    {
        void Finally<TInstance>(TInstance instance, MethodInfo methodName, object[] parameters);
    }
}
