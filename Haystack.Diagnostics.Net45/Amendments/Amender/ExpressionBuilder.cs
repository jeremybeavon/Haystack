using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Amender
{
    public static class ExpressionBuilder
    {
        public static Expression<T> BuildExpression<T>(object instance, MethodInfo method)
        {
            ParameterExpression[] parameters = method.GetParameters().Select(Parameter).ToArray();
            return Expression.Lambda<T>(Expression.Call(Expression.New(instance.GetType()), method, parameters), parameters);
        }

        private static ParameterExpression Parameter(ParameterInfo parameter)
        {
            return Expression.Parameter(parameter.ParameterType);
        }
    }
}
