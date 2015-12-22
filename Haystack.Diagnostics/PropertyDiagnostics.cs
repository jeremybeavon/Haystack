namespace Haystack.Diagnostics
{
    public static class PropertyDiagnostics<TInstance>
    {
        public static void BeforePropertyGet(TInstance instance, string propertyName)
        {
            MethodCallTraceContext.MethodCallTrace.EnterPropertyGet(instance, propertyName);
        }

        public static TProperty AfterPropertyGet<TProperty>(TInstance instance, string propertyName, TProperty returnValue)
        {
            MethodCallTraceContext.MethodCallTrace.ExitPropertyGet(returnValue);
            return returnValue;
        }

        public static TProperty BeforePropertySet<TProperty>(TInstance instance, string propertyName, TProperty oldValue, TProperty value)
        {
            MethodCallTraceContext.MethodCallTrace.EnterPropertySet(instance, propertyName, value);
            return value;
        }

        public static void AfterPropertySet<TProperty>(TInstance instance, string propertyName, TProperty oldValue,
            TProperty value, TProperty newValue)
        {
            MethodCallTraceContext.MethodCallTrace.ExitPropertySet();
        }
    }
}
