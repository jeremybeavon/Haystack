using Haystack.Diagnostics.Amendments.Amender;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments.Builder
{
    internal class PropertyAmendmentBuilderList<TType, TProperty> : IPropertyAmendmentBuilderList<TType, TProperty>
    {
        private readonly TypeAmendment typeAmendment;
        private readonly BindingFlags bindingFlags;

        public PropertyAmendmentBuilderList(TypeAmendment typeAmendment)
        {
            this.typeAmendment = typeAmendment;
        }

        public IPropertyAmendmentBuilderList<TType, TProperty> BeforePropertyGet(Expression<Action<TType, string>> beforePropertyGet)
        {
            foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.BeforePropertyGet(beforePropertyGet);
            }

            return this;
        }

        public IPropertyAmendmentBuilderList<TType, TProperty> AfterPropertyGet(Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet)
        {
			foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.AfterPropertyGet(afterPropertyGet);
            }

            return this;
        }

        public IPropertyAmendmentBuilderList<TType, TProperty> BeforePropertySet(Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet)
        {
            foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.BeforePropertySet(beforePropertySet);
            }

            return this;
        }

        public IPropertyAmendmentBuilderList<TType, TProperty> AfterPropertySet(Expression<Action<TType, TProperty, string>> afterPropertySet)
        {
            foreach (IPropertyAmendmentBuilder<TType, TProperty> property in this)
            {
                property.AfterPropertySet(afterPropertySet);
            }

            return this;
        }

        public IEnumerator<IPropertyAmendmentBuilder<TType, TProperty>> GetEnumerator()
        {
            return typeAmendment.Type.GetProperties(bindingFlags)
				.Where(property => property.PropertyType == typeof(TProperty))
				.Select(property => new PropertyAmendmentBuilder<TType, TProperty>(property)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
