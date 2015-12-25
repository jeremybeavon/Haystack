using Afterthought;
using System;
using System.Collections.Generic;

namespace Haystack.Interception.Afterthought
{
    public sealed class HaystackAmenderAttribute : Attribute, IAmendmentAttribute
    {
        public IEnumerable<ITypeAmendment> GetAmendments(Type target)
        {
            return CreateAmendment(typeof(HaystackAmender<>), target);
        }

        private static IEnumerable<ITypeAmendment> CreateAmendment(Type type, Type target)
        {
            return new[] { (ITypeAmendment)Activator.CreateInstance(type.MakeGenericType(target)) };
        }
    }
}
