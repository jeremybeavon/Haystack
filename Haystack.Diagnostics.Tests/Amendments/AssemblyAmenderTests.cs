using FluentAssertions;
using Haystack.Core.IO;
using Haystack.Diagnostics.Amendments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace Haystack.Diagnostics.Tests.Amendments
{
    [TestClass]
    public sealed class AssemblyAmenderTests
    {
        [TestMethod]
        public void TestAssemblyAmenderOnSimplePropertyAmendmentExample()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string haystackBaseDirectory = Path.Combine(baseDirectory, @"..\Haystack");
            string sourceDirectory = Path.Combine(haystackBaseDirectory, @"Examples\Amendments\SimpleProperty\Passing");
            string destinationDirectory = Path.Combine(baseDirectory, @"DiagnosticsTests\AssemblyAmender");
            string testAssembly = Path.Combine(destinationDirectory, "Haystack.Examples.Amendments.SimpleProperty.Tests.dll");
            string attributeAssembly = Path.Combine(haystackBaseDirectory, @"Runner\NUnit\3.0.1\HaystackAddin\Haystack.Runner.NUnit.dll");
            DirectoryCopy.CopyDirectory(sourceDirectory, destinationDirectory);
            Assembly assembly = Assembly.LoadFrom(attributeAssembly);
            Type attributeType = assembly.GetType("Haystack.Runner.NUnit.HaystackDiagnosticsAttribute");
            AssemblyAmender.AddAssemblyAttribute(testAssembly, attributeType);
            Assembly.LoadFrom(testAssembly).IsDefined(attributeType).Should().BeTrue();
        }
    }
}
