using System.Reflection;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Interception.Unity;

namespace Haystack.Examples.Interception.Unity.Simple.Setup
{
    public sealed class AfterConstructorAmendment : IAfterConstructorAmender
    {
        private const string typeName = "Haystack.Examples.Interception.Unity.Simple.Tests.SimpleServiceTest";

        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return constructor.DeclaringType.FullName == typeName;
        }
        
        public void AfterConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
