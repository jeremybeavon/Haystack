using AppDomainCallbackExtensions;
using System;
using System.Runtime.Serialization;

namespace Haystack.Amendments.Tests
{
    [DataContract]
    public sealed class TestCallback : CrossAppDomainFuncCallback<string>
    {
        public TestCallback()
        {
        }
        
        public TestCallback(string assemblyPath, Func<string> func)
            : base(func.Method)
        {
            AssemblyPath = assemblyPath;
        }
    }
}
