using System.Collections.Generic;

namespace Haystack.Diagnostics.ObjectModel
{
    public interface IMethodCallThreadTrace
    {
        int ThreadId { get; }

        IEnumerable<IMethodCall> MethodCalls { get; }
    }
}
