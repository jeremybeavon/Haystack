using AppDomainCallbackExtensions;
using Haystack.Diagnostics.Configuration;
using System;
using System.Runtime.Serialization;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    [DataContract]
    public sealed class TestCallback : CrossAppDomainFuncCallback<string, string>
    {
        public TestCallback()
        {
        }
        
        public TestCallback(string assemblyPath, Func<string, string> func, HaystackConfiguration configuration)
            : base(func.Method, configuration.ToString())
        {
            AssemblyPath = assemblyPath;
        }
    }
}
