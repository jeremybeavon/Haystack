using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class HaystackNonVoidMethodAmender : HaystackMethodAmender, IAfterMethodAmender, ICatchMethodAmender
    {
        public HaystackNonVoidMethodAmender(IMethodAmender amender)
            : base(amender)
        {
        }

        public override bool AmendMethod(MethodInfo method)
        {
            return method.ReturnType != typeof(void) && base.AmendMethod(method);
        }

        public TReturnValue AfterMethod<TInstance, TReturnValue>(TInstance instance, string methodName, object[] parameters, TReturnValue returnValue)
        {
            throw new NotImplementedException();
        }

        public TReturnValue CatchMethod<TInstance, TException, TReturnValue>(TInstance instance, string methodName, TException exception, object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
