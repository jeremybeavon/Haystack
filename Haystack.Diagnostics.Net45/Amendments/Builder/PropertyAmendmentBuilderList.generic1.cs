using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments.Builder
{
    internal class PropertyAmendmentBuilderList<TType> : IPropertyAmendmentBuilderList<TType>
    {
        public IPropertyAmendmentBuilderList<TType> BeforePropertyGet<TProperty>(Expression<Action<TType, string>> beforePropertyGet)
        {
            foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.BeforePropertyGet(beforePropertyGet);
            }

            return this;
        }

        public IPropertyAmendmentBuilderList<TType> AfterPropertyGet<TProperty>(Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet)
        {
			foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.AfterPropertyGet(afterPropertyGet);
            }

            return this;
        }

        public IPropertyAmendmentBuilderList<TType> BeforePropertySet<TProperty>(Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet)
        {
            foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.BeforePropertySet(beforePropertySet);
            }

            return this;
        }

        public IPropertyAmendmentBuilderList<TType> AfterPropertySet<TProperty>(Expression<Action<TType, TProperty, string>> afterPropertySet)
        {
            foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.AfterPropertySet(afterPropertySet);
            }

            return this;
        }

        public IEnumerator<IPropertyAmendmentBuilder<TType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
