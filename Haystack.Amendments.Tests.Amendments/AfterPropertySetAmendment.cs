using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Amendments.Tests.Amendments
{
    public sealed class AfterPropertySetAmendment : IAfterPropertySetAmender
    {
        public bool AmendProperty(PropertyInfo property)
        {
            return true;
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return true;
        }
        
        public void AfterPropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            const string format = "AfterPropertySet(instance = {0}, propertyName = {1}, value = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, propertyName, value);
        }
    }
}
