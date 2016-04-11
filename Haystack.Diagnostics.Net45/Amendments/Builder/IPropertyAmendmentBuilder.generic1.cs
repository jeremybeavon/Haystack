using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Builder
{
    public interface IPropertyAmendmentBuilder<TType>
    {
        PropertyInfo Property { get; }

        IPropertyAmendmentBuilder<TType> BeforePropertyGet(Expression<Action<TType, string>> beforePropertyGet);

        IPropertyAmendmentBuilder<TType> AfterPropertyGet<TProperty>(
            Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet);

        IPropertyAmendmentBuilder<TType> BeforePropertySet<TProperty>(
            Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet);

        IPropertyAmendmentBuilder<TType> AfterPropertySet<TProperty>(
            Expression<Action<TType, TProperty, string>> afterPropertyGet);
    }
}
