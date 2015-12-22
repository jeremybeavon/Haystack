using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Runner.NUnit
{
    public static class MethodInfoToDelegateExtensions
    {
        private static readonly ConcurrentDictionary<MethodInfo, object> cachedDelegates = new ConcurrentDictionary<MethodInfo, object>();

        public static T ToDelegate<T>(this MethodInfo method)
        {
            return method.IsStatic ?
                (T)(object)Delegate.CreateDelegate(typeof(T), method) :
                (T)cachedDelegates.GetOrAdd(method, method2 => CreateDelegate<T>(method2));
        }

        public static T ToDelegate<T>(this MethodInfo method, object target)
        {
            return (T)(object)Delegate.CreateDelegate(typeof(T), target, method);
        }

        private static T CreateDelegate<T>(MethodInfo method)
        {
            if (method.DeclaringType == null)
            {
                throw new ArgumentException("method.DeclaringType cannot be null", "method");
            }

            ParameterExpression instanceParameter = Expression.Parameter(method.DeclaringType);
            ParameterExpression[] parameters = method.GetParameters().Select(GetParameterExpression).ToArray();
            Expression body = Expression.Call(instanceParameter, method, parameters.Cast<Expression>());
            return Expression.Lambda<T>(body, new[] { instanceParameter }.Concat(parameters)).Compile();
        }

        private static ParameterExpression GetParameterExpression(ParameterInfo parameter)
        {
            return Expression.Parameter(parameter.ParameterType, parameter.Name);
        }
    }
}
