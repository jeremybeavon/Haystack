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
        
        public void BeforeConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            MethodCallTraceContext.MethodCallTrace.EnterConstructorCall(instance, constructor, parameters);
        }

        public void AfterConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            MethodCallTraceContext.MethodCallTrace.ExitConstructorCall(parameters);
        }
    }
}
