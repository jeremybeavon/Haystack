using System.Collections.Generic;

namespace Haystack.Diagnostics.ObjectModel
{
    public interface IMethodCallTrace
    {
        string Description { get; }

        IEnumerable<IMethodCallThreadTrace> MethodCallThreads { get; }

        IEnumerable<IObjectInstance> Objects { get; }

        IEnumerable<IObjectType> Types { get; }
    }
}
