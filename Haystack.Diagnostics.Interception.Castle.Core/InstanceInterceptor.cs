using Castle.DynamicProxy;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Diagnostics.Interception.Castle.Core
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
            MethodCallTraceContext.MethodCallTrace.EnterMethodCall(
                methodInvocation.InvocationTarget,
                methodInvocation.Method,
                methodInvocation.Arguments);
        }

        private void ExitMethodCall(IInvocation methodInvocation)
        {
            MethodCallTraceContext.MethodCallTrace.ExitMethodCall(methodInvocation.ReturnValue, methodInvocation.Arguments);
        }
    }
}
