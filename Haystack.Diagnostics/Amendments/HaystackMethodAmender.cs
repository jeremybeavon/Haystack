using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal abstract class HaystackMethodAmender : IBeforeMethodAmender
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

        public bool AmendMethod(Type type, string methodName, object[] parameters)
        {
            return methodAmender.AmendMethod(type, methodName, parameters);
        }

        public void BeforeMethod<TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
