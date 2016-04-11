using Haystack.Diagnostics.Amendments.Amender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments.Builder
{
    internal sealed class TypeAmendmentBuilder<TType> : ITypeAmendmentBuilder<TType>
    {
        private readonly TypeAmendment typeAmendment;
        private readonly BindingFlags bindingFlags;

        public TypeAmendmentBuilder(TypeAmendment typeAmendment)
            : this(typeAmendment, BindingFlags.Public)
        {
        }

        public TypeAmendmentBuilder(TypeAmendment typeAmendment, BindingFlags bindingFlags)
        {
            this.typeAmendment = typeAmendment;
            this.bindingFlags = bindingFlags;
        }

        public IPropertyAmendmentBuilderList<TType, TProperty> Properties<TProperty>()
        {
            return new PropertyAmendmentBuilderList<TType, TProperty>(typeAmendment, bindingFlags)
        }

        public IPropertyAmendmentBuilderList<TType> Properties()
        {

        }
    }
}
