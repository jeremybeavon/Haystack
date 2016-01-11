using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;
using Haystack.Interception.Castle.Windsor;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple.Setup
{
    public sealed class BeforeMethodAmendment : IBeforeMethodAmender
    {
        private const string typeName = "Haystack.Examples.Interception.Castle.Windsor.Simple.Tests.SimpleServiceTest";

        public bool AmendMethod(MethodInfo method)
        {
            return method.DeclaringType.FullName == typeName && method.Name == "SetUp";
        }

        public bool AmendMethod(Type type, string methodName, object[] parameters)
        {
            return type.FullName == typeName && methodName == "SetUp";
        }

        public void BeforeMethod<TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
