using System;
using System.Reflection;
using Haystack.Diagnostics.Amendments;
using Haystack.Interception.Ninject;

namespace Haystack.Examples.Interception.Ninject.Simple.Setup
{
    public sealed class AfterConstructorAmendment : IAfterConstructorAmender
    {
        private const string typeName = "Haystack.Examples.Interception.Ninject.Simple.Tests.SimpleServiceTest";

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
            HaystackInterceptor.SetUp(DependencyManager.SimpleKernel);
        }
    }
}
