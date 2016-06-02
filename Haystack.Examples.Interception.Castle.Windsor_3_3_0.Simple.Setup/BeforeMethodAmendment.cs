using System.Reflection;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Interception.Castle.Windsor;

namespace Haystack.Examples.Interception.Castle.Windsor.Simple.Setup
{
    public sealed class BeforeMethodAmendment : IBeforeMethodAmender
    {
        private const string typeName = "Haystack.Examples.Interception.Castle.Windsor.Simple.Tests.SimpleServiceTest";

        public bool AmendMethod(MethodInfo method)
        {
            return method.DeclaringType.FullName == typeName && method.Name == "SetUp";
        }
        
        public void BeforeMethod<TInstance>(TInstance instance, MethodInfo method, object[] parameters)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
