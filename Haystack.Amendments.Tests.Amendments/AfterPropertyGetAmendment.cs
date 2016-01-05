using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Amendments.Tests.Amendments
{
    public sealed class AfterPropertyGetAmendment : IAfterPropertyGetAmender
    {
        public bool AmendProperty(PropertyInfo property)
        {
            return true;
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return true;
        }

        public TProperty AfterPropertyGet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            const string format = "AfterPropertyGet(instance = {0}, propertyName = {1}, value = {2})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, propertyName, value);
            return value;
        }
    }
}
