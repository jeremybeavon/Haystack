using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Builder
{
    public interface IPropertyAmendmentBuilder<TType, TProperty>
    {
        PropertyInfo Property { get; }

        IPropertyAmendmentBuilder<TType, TProperty> BeforePropertyGet(Expression<Action<TType, string>> beforePropertyGet);

        IPropertyAmendmentBuilder<TType, TProperty> AfterPropertyGet(Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet);

        IPropertyAmendmentBuilder<TType, TProperty> BeforePropertySet(Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet);

        IPropertyAmendmentBuilder<TType, TProperty> AfterPropertySet(Expression<Action<TType, TProperty, string>> afterPropertySet);
    }
}
