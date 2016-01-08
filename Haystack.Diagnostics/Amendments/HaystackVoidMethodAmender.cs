using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class HaystackVoidMethodAmender : HaystackMethodAmender, IAfterVoidMethodAmender, ICatchVoidMethodAmender
    {
        public HaystackVoidMethodAmender(IMethodAmender amender)
            : base(amender)
        {
        }

        public override bool AmendMethod(MethodInfo method)
        {
            return method.ReturnType == typeof(void) && base.AmendMethod(method);
        }

        public void AfterMethod<TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void CatchMethod<TInstance, TException>(TInstance instance, string methodName, TException exception, object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
