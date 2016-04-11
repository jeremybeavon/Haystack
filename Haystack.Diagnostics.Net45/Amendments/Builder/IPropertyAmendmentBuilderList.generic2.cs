using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Haystack.Diagnostics.Amendments.Builder
{
    public interface IPropertyAmendmentBuilderList<TType, TProperty> : IEnumerable<IPropertyAmendmentBuilder<TType, TProperty>>
    {
        IPropertyAmendmentBuilderList<TType, TProperty> BeforePropertyGet(Expression<Action<TType, string>> beforePropertyGet);

        IPropertyAmendmentBuilderList<TType, TProperty> AfterPropertyGet(Expression<Func<TType, TProperty, string, TProperty>> afterPropertyGet);

        IPropertyAmendmentBuilderList<TType, TProperty> BeforePropertySet(Expression<Func<TType, TProperty, string, TProperty>> beforePropertySet);

        IPropertyAmendmentBuilderList<TType, TProperty> AfterPropertySet(Expression<Action<TType, TProperty, string>> afterPropertyGet);
    }
}
