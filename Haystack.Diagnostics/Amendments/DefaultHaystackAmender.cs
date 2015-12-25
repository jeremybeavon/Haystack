using System;

namespace Haystack.Diagnostics.Amendments
{
    /*internal sealed class DefaultHaystackAmender : CustomHaystackAmender
    {
        private readonly IHaystackAmender amender;

        public DefaultHaystackAmender(IHaystackAmender amender)
        {
            this.amender = amender;
        }

        public override bool AmendProperty(Type type, string propertyName)
        {
            return amender.AmendProperty(type, propertyName);
        }

        public override TProperty AfterPropertyGet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.ExitPropertyGet(value);
            return value;
        }

        public override void AfterPropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.ExitPropertySet();
        }

        public override void BeforePropertyGet<TInstance>(TInstance instance, string propertyName)
        {
            MethodCallTraceContext.MethodCallTrace.EnterPropertyGet(instance, propertyName);
        }

        public override TProperty BeforePropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.EnterPropertySet(instance, propertyName, value);
            return value;
        }
    }*/
}
