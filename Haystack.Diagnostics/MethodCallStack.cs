using System.Collections.Generic;
using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Diagnostics
{
    internal sealed class MethodCallStack
    {
        public MethodCallStack()
        {
            CallStack = new Stack<MethodCall>(new[] { new MethodCall() });
        }
        
        public Stack<MethodCall> CallStack { get; set; }

        public bool IsInGetHashCode { get; set; }
    }
}
