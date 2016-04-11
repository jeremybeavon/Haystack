using Afterthought;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public static class AmendmentExtensions
    {
        public static Amendment<TType, TAmended>.PropertyEnumeration Where<TType, TAmended, TAmender>(
            this Amendment<TType, TAmended>.PropertyList properties,
            IEnumerable<TAmender> amenders)
            where TAmender : IPropertyAmender
        {
            return properties
                .Where(property => !IsGenericType(property.PropertyInfo))
                .Where(property => amenders.Any(amender => AmendProperty(amender, property.PropertyInfo)));
        }

        public static Amendment<TType, TAmended>.ConstructorEnumeration Where<TType, TAmended, TAmender>(
            this Amendment<TType, TAmended>.ConstructorList constructors,
            IEnumerable<TAmender> amenders)
            where TAmender : IConstructorAmender
        {
            return constructors
                .Where(constructor => !IsGenericType(constructor.ConstructorInfo))
                .Where(constructor => amenders.Any(amender => AmendConstructor(amender, constructor.ConstructorInfo)));
        }

        public static Amendment<TType, TAmended>.MethodEnumeration Where<TType, TAmended, TAmender>(
            this Amendment<TType, TAmended>.MethodList methods,
            IEnumerable<TAmender> amenders)
            where TAmender : IMethodAmender
        {
            return methods
                .Where(method => !IsGenericType(method.MethodInfo) && !method.MethodInfo.IsGenericMethod)
                .Where(method => amenders.Any(amender => AmendMethod(amender, method.MethodInfo)));
        }

        private static bool IsGenericType(MemberInfo member)
        {
            return member.DeclaringType != null && member.DeclaringType.IsGenericType;
        }

        private static bool AmendProperty(IPropertyAmender amender, PropertyInfo property)
        {
            return amender.AmendProperty(property.DeclaringType, property.Name);
        }

        private static bool AmendConstructor(IConstructorAmender amender, ConstructorInfo constructor)
        {
            return amender.AmendConstructor(constructor.DeclaringType, new object[constructor.GetParameters().Length]);
        }

        private static bool AmendMethod(IMethodAmender amender, MethodInfo method)
        {
            return amender.AmendMethod(method.DeclaringType, method.Name, new object[method.GetParameters().Length]);
        }
    }
}
