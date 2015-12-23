using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Registry;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Interception.Ninject
{
    public class HaystackInterceptor : IInterceptor
    {
        public static void Setup(IKernel kernel)
        {
            IAdvice advice = kernel.Components.Get<IAdviceFactory>().Create(context => context.Binding.Service.IsInterface);
            advice.Interceptor = new HaystackInterceptor();
            kernel.Components.Get<IAdviceRegistry>().Register(advice);
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
            MethodInfo method = methodInvocation.Request.Method;
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = provider.GetTypeIndex(method.DeclaringType),
                InstanceIndex = provider.GetObjectIndex(methodInvocation.Request.Target),
                MethodName = method.Name,
                Parameters = provider.GetParameters(methodInvocation.Request.Arguments, method.GetParameters()),
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
                methodCall.Parameters[index].OutputValue = provider.GetValue(methodInvocation.Request.Arguments[index]);
            }
        }
    }
}
