using System;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class HaystackPropertyAmender :
        IBeforePropertyGetAmender,
        IAfterPropertyGetAmender,
        IBeforePropertySetAmender,
        IAfterPropertySetAmender
    {
        private readonly IPropertyAmender amender;

        public HaystackPropertyAmender(IPropertyAmender amender)
        {
            this.amender = amender;
        }

        public bool AmendProperty(PropertyInfo property)
        {
            return amender.AmendProperty(property);
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return amender.AmendProperty(type, propertyName);
        }

        public void BeforePropertyGet<TInstance>(TInstance instance, string propertyName)
        {
            MethodCallTraceContext.MethodCallTrace.EnterPropertyGet(instance, propertyName);
        }

        public TProperty AfterPropertyGet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.ExitPropertyGet(value);
            return value;
        }

        public TProperty BeforePropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.EnterPropertySet(instance, propertyName, value);
            return value;
        }

        public void AfterPropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.ExitPropertySet();
        }
    }
}
