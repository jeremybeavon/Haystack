using Haystack.Interception.Castle.Core;
using StructureMap;
using StructureMap.Building.Interception;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Interception.StructureMap
{
    public class HaystackInterceptionPolicy : IInterceptorPolicy
    {
        public string Description
        {
            get { return "Haystack"; }
        }

        public static void SetUp(IContainer container)
        {
            container.Model.Pipeline.Policies.Add(new HaystackInterceptionPolicy());
        }

        public IEnumerable<IInterceptor> DetermineInterceptors(Type pluginType, Instance instance)
        {
            if (pluginType.IsInterface)
            {
                ParameterExpression parameter = Expression.Parameter(pluginType);
                MethodInfo method = new Func<Type, object, Type[], object>(InstanceInterceptor.CreateInstance).Method;
                Expression methodCall = Expression.Call(method, Expression.Constant(pluginType), parameter, Expression.Constant(Type.EmptyTypes));
                Type delegateType = typeof(Func<,>).MakeGenericType(pluginType, pluginType);
                Expression body = Expression.Lambda(delegateType, Expression.Convert(methodCall, pluginType), parameter);
                Type interceptorType = typeof(FuncInterceptor<>).MakeGenericType(pluginType);
                yield return (IInterceptor)Activator.CreateInstance(interceptorType, body, null);
            }
        }
    }
}
