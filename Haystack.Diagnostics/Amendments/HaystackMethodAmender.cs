using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal abstract class HaystackMethodAmender
    {
        private readonly IMethodAmender methodAmender;

        protected HaystackMethodAmender(IMethodAmender methodAmender)
        {
            this.methodAmender = methodAmender;
        }

        public virtual bool AmendMethod(MethodInfo method)
        {
            return methodAmender.AmendMethod(method);
        }
    }
}
