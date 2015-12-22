using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Interception.Unity
{
    public sealed class HaystackInterceptionBehaviour : IInterceptionBehavior
    {
        public bool WillExecute
        {
            get { return true; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            EnterMethodCall(input);
            IMethodReturn returnValue = getNext()(input, getNext);
            ExitMethodCall(returnValue);
            return returnValue;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        private void EnterMethodCall(IMethodInvocation methodInvocation)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodCall methodCall = new MethodCall
            {
                DeclaringTypeIndex = provider.GetTypeIndex(methodInvocation.MethodBase.DeclaringType),
                InstanceIndex = provider.GetObjectIndex(methodInvocation.Target),
                MethodName = methodInvocation.MethodBase.Name,
                Parameters = provider.GetParameters(methodInvocation.Arguments.Cast<object>(), methodInvocation.MethodBase.GetParameters()),
                ReturnTypeIndex = provider.GetTypeIndex(((MethodInfo)methodInvocation.MethodBase).ReturnType)
            };
            provider.EnterMethodCall(methodCall);
        }

        private void ExitMethodCall(IMethodReturn returnValue)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodCall methodCall = provider.ExitMethodCall();
            methodCall.ReturnValue = provider.GetValue(returnValue.ReturnValue);
            foreach (MethodParameter parameter in methodCall.Parameters.Where(param => param.Modifier != ParameterModifier.None))
            {
                parameter.OutputValue = provider.GetValue(returnValue.Outputs.GetParameterInfo(parameter.ParameterName));
            }
        }
    }
}
