using Haystack.Diagnostics.ObjectModel;
using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class HaystackConstructorAmender : IBeforeConstructorAmender, IAfterConstructorAmender
    {
        private readonly IConstructorAmender amender;

        public HaystackConstructorAmender(IConstructorAmender amender)
        {
            this.amender = amender;
        }

        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return amender.AmendConstructor(constructor);
        }

        public bool AmendConstructor(Type type, object[] parameters)
        {
            return amender.AmendConstructor(type, parameters);
        }

        public void BeforeConstructor<TInstance>(TInstance instance, object[] parameters)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodCall methodCall = new MethodCall()
            {
            };
            provider.EnterMethodCall(methodCall);
        }

        public void AfterConstructor<TInstance>(TInstance instance, object[] parameters)
        {
            MethodCallTraceContext.MethodCallTrace.ExitMethodCall();
        }
    }
}
