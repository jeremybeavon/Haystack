using Castle.DynamicProxy;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Interception.Castle.Core
{
    public sealed class InstanceInterceptor : IInterceptor
    {
        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
        private static readonly IInterceptor instanceInterceptor = new InstanceInterceptor();

        public static object CreateInstance(Type interfaceType, object target, params Type[] additionalInterfaceTypes)
        {
            return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, additionalInterfaceTypes, target, instanceInterceptor);
        }

        public void Intercept(IInvocation invocation)
        {
            EnterMethodCall(invocation);
            invocation.Proceed();
            ExitMethodCall(invocation);
        }

        private void EnterMethodCall(IInvocation methodInvocation)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodInfo method = methodInvocation.Method;
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = provider.GetTypeIndex(method.DeclaringType),
                InstanceIndex = provider.GetObjectIndex(methodInvocation.InvocationTarget),
                MethodName = method.Name,
                Parameters = provider.GetParameters(methodInvocation.Arguments, method.GetParameters()),
                ReturnTypeIndex = provider.GetTypeIndex(method.ReturnType),
            };
            if (method.Attributes.HasFlag(MethodAttributes.SpecialName) && Regex.IsMatch(method.Name, "^[gs]et_"))
            {
                methodCall.PropertyType = method.Name.StartsWith("get_") ? PropertyType.Get : PropertyType.Set;
                methodCall.MethodName = methodCall.MethodName.Substring(4);
            }

            provider.EnterMethodCall(methodCall);
        }

        private void ExitMethodCall(IInvocation methodInvocation)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodCall methodCall = provider.ExitMethodCall();
            methodCall.ReturnValue = provider.GetValue(methodInvocation.ReturnValue);
            foreach (int index in methodCall.Parameters.Where(param => param.Modifier != ParameterModifier.None).Select((value, index) => index))
            {
                methodCall.Parameters[index].OutputValue = provider.GetValue(methodInvocation.Arguments[index]);
            }
        }
    }
}
