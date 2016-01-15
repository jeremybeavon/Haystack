using Afterthought;
using System;
using System.Collections.Generic;

namespace Haystack.Diagnostics.Amendments
{
    public sealed class HaystackAmenderAttribute : Attribute, IAmendmentAttribute
    {
        public IEnumerable<ITypeAmendment> GetAmendments(Type target)
        {
            AmenderInitializer.InitializeIfNecessary();
            return CreateAmendment(typeof(HaystackAmender<>), target);
        }

        private static IEnumerable<ITypeAmendment> CreateAmendment(Type type, Type target)
        {
            return new[] { (ITypeAmendment)Activator.CreateInstance(type.MakeGenericType(target)) };
        }
    }
}
