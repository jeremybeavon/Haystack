using Castle.DynamicProxy;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Interception.Castle.Core
{
    public sealed class InstanceInterceptor : IInterceptor
    {
        public static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

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
                DeclaringTypeIndex = provider.GetTypeIndex(methodInvocation.Method.DeclaringType),
                InstanceIndex = provider.GetObjectIndex(methodInvocation.InvocationTarget),
                MethodName = methodInvocation.Method.Name,
                Parameters = provider.GetParameters(methodInvocation.Arguments, methodInvocation.Method.GetParameters()),
                ReturnTypeIndex = provider.GetTypeIndex(methodInvocation.Method.ReturnType),
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
