using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;
using Haystack.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class AfterConstructorAmendmentWithContainer : IAfterConstructorAmender
    {
        private const string typeName = "Haystack.Examples.Interception.Autofac.Simple.Tests.SimpleServiceTest";

        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return constructor.DeclaringType.FullName == typeName;
        }

        public bool AmendConstructor(Type type, object[] parameters)
        {
            return type.FullName == typeName;
        }

        public void AfterConstructor<TInstance>(TInstance instance, object[] parameters)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainerBuilder);
        }
    }
}
