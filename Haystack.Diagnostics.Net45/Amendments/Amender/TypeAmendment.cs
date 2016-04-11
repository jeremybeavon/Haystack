using Mono.Cecil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics.Amendments.Amender
{
    internal sealed class TypeAmendment
    {
        private readonly ConcurrentDictionary<PropertyInfo, PropertyAmendment> propertyAmendments;

        public TypeAmendment(Type type)
        {
            propertyAmendments = new ConcurrentDictionary<PropertyInfo, PropertyAmendment>();
        }

        public Type Type { get; private set; }

        public PropertyAmendment GetPropertyAmendment(PropertyInfo property)
        {
            return propertyAmendments.GetOrAdd(property, prop => new PropertyAmendment(prop));
        }

        public PropertyAmendment GetPropertyAmendment(PropertyDefinition property)
        {
            return null;
        }
    }
}
