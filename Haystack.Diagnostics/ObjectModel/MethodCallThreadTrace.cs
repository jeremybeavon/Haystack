using System.Collections.Generic;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class MethodCallThreadTrace
    {
        public MethodCallThreadTrace()
        {
            MethodCalls = new List<MethodCall>();
        }

        public MethodCallThreadTrace(int threadId, List<MethodCall> methodCalls)
        {
            ThreadId = threadId;
            MethodCalls = methodCalls;
        }

        public int ThreadId { get; set; }

        public List<MethodCall> MethodCalls { get; set; }
    }
}
