using Haystack.Diagnostics.Amendments.Amender;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Builder
{
    internal sealed class PropertyAmendmentBuilder<TType, TProperty> : IPropertyAmendmentBuilder<TType, TProperty>
    {
        private readonly TypeAmendment typeAmendment;
        private readonly PropertyInfo property;
        
        public PropertyAmendmentBuilder(TypeAmendment typeAmendment, PropertyInfo property)
        {
            this.typeAmendment = typeAmendment;
            this.property = property;
        }

        public PropertyInfo Property
        {
            get { return property; }
        }

        private PropertyAmendment PropertyAmendment
        {
            get { return typeAmendment.GetPropertyAmendment(property); }
        }

        public IPropertyAmendmentBuilder<TType, TProperty> BeforePropertyGet(Expression<Action<TType, string>> beforePropertyGet)
        {
            PropertyAmendment.BeforePropertyGetExpressions.Add(beforePropertyGet);
            return this;
        }

        public IPropertyAmendmentBuilder<TType, TProperty> AfterPropertyGet(Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet)
        {
            PropertyAmendment.AfterPropertyGetExpressions.Add(afterPropertyGet);
            return this;
        }

        public IPropertyAmendmentBuilder<TType, TProperty> BeforePropertySet(Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet)
        {
            PropertyAmendment.BeforePropertySetExpressions.Add(beforePropertySet);
            return this;
        }

        public IPropertyAmendmentBuilder<TType, TProperty> AfterPropertySet(Expression<Action<TType, TProperty, string>> afterPropertyGet)
        {
            PropertyAmendment.AfterPropertySetExpressions.Add(afterPropertyGet);
            return this;
        }
    }
}
