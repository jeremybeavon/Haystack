using Haystack.Diagnostics.Amendments.Amender;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments.Builder
{
    public abstract class AmendmentBuilder
    {
        private readonly ConcurrentDictionary<Type, TypeAmendment> typeAmendments;

        public AmendmentBuilder()
        {
            typeAmendments = new ConcurrentDictionary<Type, TypeAmendment>();
        }

        protected ITypeAmendmentBuilder<TType> AmendType<TType>()
        {
            return new TypeAmendmentBuilder<TType>(GetTypeAmendment(typeof(TType)));
        }

        protected ITypeAmendmentBuilder<TType> AmendType<TType>(BindingFlags bindingFlags)
        {
            return new TypeAmendmentBuilder<TType>(GetTypeAmendment(typeof(TType)), )
        }

        private TypeAmendment GetTypeAmendment(Type type)
        {
            return typeAmendments.GetOrAdd(type, newType => new TypeAmendment(newType));
        }
    }
}
