using Afterthought;
using Haystack.Diagnostics.Amendments;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Interception.Afterthought
{
    public static class AmendmentExtensions
    {
        public static Amendment<TType, TAmended>.PropertyEnumeration Where<TType, TAmended, TAmender>(
            this Amendment<TType, TAmended>.PropertyList properties,
            IEnumerable<TAmender> amenders)
            where TAmender : IPropertyAmender
        {
            return properties.Where(property => amenders.Any(amender => amender.AmendProperty(property.PropertyInfo)));
        }

        public static Amendment<TType, TAmended>.ConstructorEnumeration Where<TType, TAmended, TAmender>(
            this Amendment<TType, TAmended>.ConstructorList constructors,
            IEnumerable<TAmender> amenders)
            where TAmender : IConstructorAmender
        {
            return constructors.Where(constructor => amenders.Any(amender => amender.AmendConstructor(constructor.ConstructorInfo)));
        }

        public static Amendment<TType, TAmended>.MethodEnumeration Where<TType, TAmended, TAmender>(
            this Amendment<TType, TAmended>.MethodList methods,
            IEnumerable<TAmender> amenders)
            where TAmender : IMethodAmender
        {
            return methods.Where(method => amenders.Any(amender => amender.AmendMethod(method.MethodInfo)));
        }
    }
}
