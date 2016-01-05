﻿using System.Collections.Generic;
using System.Linq;

namespace Haystack.Diagnostics.Amendments
{
    public static class PropertyAmendments<TInstance>
    {
        public static void BeforePropertyGet(TInstance instance, string propertyName)
        {
            IEnumerable<IBeforePropertyGetAmender> amenders = AmendmentRepository.BeforePropertyGetAmenders;
            if (amenders != null)
            {
                foreach (IBeforePropertyGetAmender amender in GetAmenders(amenders, propertyName))
                {
                    amender.BeforePropertyGet(instance, propertyName);
                }
            }
        }

        public static TProperty AfterPropertyGet<TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            return GetAmenders(AmendmentRepository.AfterPropertyGetAmenders, propertyName)
                .Aggregate(value, (returnValue, amender) => amender.AfterPropertyGet(instance, propertyName, returnValue));
        }

        public static TProperty BeforePropertySet<TProperty>(TInstance instance, string propertyName, TProperty oldValue, TProperty value)
        {
            return GetAmenders(AmendmentRepository.BeforePropertySetAmenders, propertyName)
                .Aggregate(value, (returnValue, amender) => amender.BeforePropertySet(instance, propertyName, returnValue));
        }

        public static void AfterPropertySet<TProperty>(TInstance instance, string propertyName, TProperty oldValue,
            TProperty value, TProperty newValue)
        {
            IEnumerable<IAfterPropertySetAmender> amenders = AmendmentRepository.AfterPropertySetAmenders;
            if (amenders != null)
            {
                foreach (IAfterPropertySetAmender amender in GetAmenders(amenders, propertyName))
                {
                    amender.AfterPropertySet(instance, propertyName, value);
                }
            }
        }

        private static IEnumerable<TAmender> GetAmenders<TAmender>(IEnumerable<TAmender> amenders, string propertyName)
            where TAmender : IPropertyAmender
        {
            return amenders == null ? new TAmender[0] : amenders.Where(amender => amender.AmendProperty(typeof(TInstance), propertyName));
        }
    }
}
