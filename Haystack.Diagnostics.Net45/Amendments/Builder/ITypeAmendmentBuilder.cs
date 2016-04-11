using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments.Builder
{
    public interface ITypeAmendmentBuilder<TType>
    {
        IPropertyAmendmentBuilderList<TType, TProperty> Properties<TProperty>();

        IPropertyAmendmentBuilderList<TType> Properties();
    }
}
