using Haystack.Bootstrap;
using Haystack.Diagnostics.ObjectModel;
using PostSharp.Aspects;
using System;
using System.Linq;

namespace Haystack.Diagnostics.Amendments.PostSharp
{
    [Serializable]
    public sealed class HaystackExitAspect : OnMethodBoundaryAspect
    {
        public HaystackExitAspect()
        {
            AspectPriority = int.MaxValue;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            AfterMethodCall(args);
        }

        private void AfterMethodCall(MethodExecutionArgs args)
        {
            MethodCallTraceProvider provider = MethodCallTraceContext.MethodCallTrace;
            MethodCall methodCall = provider.ExitMethodCall();
            methodCall.ReturnValue = provider.GetValue(args.ReturnValue);
            foreach (int index in methodCall.Parameters.Where(param => param.Modifier != ParameterModifier.None).Select((value, index) => index))
            {
                methodCall.Parameters[index].OutputValue = provider.GetValue(args.Arguments[index]);
            }
        }
    }
}
