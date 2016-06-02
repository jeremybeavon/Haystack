using System.Reflection;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class AfterConstructorAmendmentWithContainerBuilder : IAfterConstructorAmender
    {
        private const string typeName = "Haystack.Examples.Interception.Autofac.Simple.Tests.SimpleServiceTest";

        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return constructor.DeclaringType.FullName == typeName;
        }
        
        public void AfterConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainerBuilder);
        }
    }
}
