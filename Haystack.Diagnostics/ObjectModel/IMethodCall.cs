using System.Collections.Generic;

namespace Haystack.Diagnostics.ObjectModel
{
    public interface IMethodCall
    {
        int Index { get; }

        IObjectType DeclaringType { get; }

        IObjectInstance Instance { get; }

        string MethodName { get; }

        IEnumerable<IMethodParameter> Parameters { get; }

        PropertyType PropertyType { get; }

        IObjectType ReturnType { get; }

        IValue ReturnValue { get; }

        IEnumerable<IMethodCall> MethodCalls { get; }

        IMethodCall CalledBy { get; }

        IMethodCallThreadTrace Thread { get; }

        IMethodCallTrace Trace { get; }

        MethodCall ToMethodCall();
    }
}
