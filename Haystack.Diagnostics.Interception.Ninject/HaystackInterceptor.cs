using Haystack.Diagnostics.ObjectModel;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Registry;
using Ninject.Extensions.Interception.Request;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Diagnostics.Interception.Ninject
{
    public class HaystackInterceptor : IInterceptor
    {
        public static void SetUp(IKernel kernel)
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
            IProxyRequest request = methodInvocation.Request;
            MethodCallTraceContext.MethodCallTrace.EnterMethodCall(request.Target, request.Method, request.Arguments);
        }

        private void ExitMethodCall(IInvocation methodInvocation)
        {
            MethodCallTraceContext.MethodCallTrace.ExitMethodCall(methodInvocation.ReturnValue, methodInvocation.Request.Arguments);
        }
    }
}
