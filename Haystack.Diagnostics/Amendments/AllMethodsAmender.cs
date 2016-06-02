using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public class AllMethodsAmender : IMethodAmender
    {
        public bool AmendMethod(MethodInfo method)
        {
            return true;
        }
    }
}
