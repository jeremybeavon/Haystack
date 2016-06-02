using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IMethodAmender
    {
        bool AmendMethod(MethodInfo method);
    }
}
