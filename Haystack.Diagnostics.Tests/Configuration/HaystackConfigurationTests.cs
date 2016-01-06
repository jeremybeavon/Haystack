using Haystack.Diagnostics.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Haystack.Diagnostics.Tests.Configuration
{
    [TestClass]
    public sealed class HaystackConfigurationTests
    {
        [TestMethod]
        public void TestMethod()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName name = assembly.GetName();
            string test = (new HaystackConfiguration()
            {
                Amendments = new AmendmentConfiguration()
                {
                    AssembliesToAmend = new List<string>() { "Test.dll" }
                }
            }).ToString();
            test.GetHashCode();
        }
    }
}
