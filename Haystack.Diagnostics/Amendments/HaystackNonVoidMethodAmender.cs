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

        public TReturnValue AfterMethod<TInstance, TReturnValue>(TInstance instance, MethodInfo method, object[] parameters, TReturnValue returnValue)
        {
            MethodCallTraceContext.MethodCallTrace.ExitMethodCall(returnValue, parameters);
            return returnValue;
        }

        public TReturnValue CatchMethod<TInstance, TException, TReturnValue>(TInstance instance, MethodInfo method, TException exception, object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
