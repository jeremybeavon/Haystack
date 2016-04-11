using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Haystack.Diagnostics.Amendments.Builder
{
    public interface IPropertyAmendmentBuilderList<TType> : IEnumerable<IPropertyAmendmentBuilder<TType>>
    {
        IPropertyAmendmentBuilderList<TType> BeforePropertyGet(Expression<Action<TType, string>> beforePropertyGet);

        IPropertyAmendmentBuilderList<TType> AfterPropertyGet<TProperty>(
            Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet);

        IPropertyAmendmentBuilderList<TType> BeforePropertySet<TProperty>(
            Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet);

        IPropertyAmendmentBuilderList<TType> AfterPropertySet<TProperty>(
            Expression<Action<TType, TProperty, string>> afterPropertyGet);
    }
}
