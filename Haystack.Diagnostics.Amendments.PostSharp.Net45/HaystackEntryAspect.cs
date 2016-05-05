using Haystack.Bootstrap;
using Haystack.Diagnostics.ObjectModel;
using PostSharp.Aspects;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Haystack.Diagnostics.Amendments.PostSharp
{
    [Serializable]
    public sealed class HaystackEntryAspect : OnMethodBoundaryAspect
    {
        public HaystackEntryAspect()
        {
            AspectPriority = int.MinValue;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            BeforeMethodCall(args);
        }

        private void BeforeMethodCall(MethodExecutionArgs args)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodBase method = args.Method;
            MethodInfo methodInfo = method as MethodInfo;
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = provider.GetTypeIndex(method.DeclaringType),
                InstanceIndex = provider.GetObjectIndex(args.Instance),
                MethodName = method.Name,
                Parameters = provider.GetParameters(args.Arguments, method.GetParameters()),
            };
            if (methodInfo != null)
            {
                methodCall.ReturnTypeIndex = provider.GetTypeIndex(methodInfo.ReturnType);
            }

            if (method.Attributes.HasFlag(MethodAttributes.SpecialName) && Regex.IsMatch(method.Name, "^[gs]et_"))
            {
                methodCall.PropertyType = method.Name.StartsWith("get_") ? PropertyType.Get : PropertyType.Set;
                methodCall.MethodName = methodCall.MethodName.Substring(4);
            }

            provider.EnterMethodCall(methodCall);
        }
    }
}
