using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace Haystack.Interception.Unity
{
    public sealed class HaystackInterceptor : UnityContainerExtension, IBuilderStrategy
    {
        private readonly IUnityContainer container;
        private readonly InjectionMember[] injectionMembers;

        public HaystackInterceptor(IUnityContainer container)
        {
            this.container = container;
            injectionMembers = new InjectionMember[]
            {
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<HaystackInterceptionBehaviour>()
            };
        }

        public static void SetUp(IUnityContainer container)
        {
            container.AddExtension(new HaystackInterceptor(container));
        }

        public void PreBuildUp(IBuilderContext context)
        {
            NamedTypeBuildKey buildKey = context.BuildKey;
            Type type = buildKey.Type;
            if (type.IsInterface && !type.IsGenericType && type.Name.StartsWith("I"))
            {
                IBuildKeyMappingPolicy buildKeyMappingPolicy = context.PersistentPolicies.Get<IBuildKeyMappingPolicy>(buildKey);
                if (buildKeyMappingPolicy == null)
                {
                    Type classType = GetClassType(type);
                    if (classType != null)
                    {
                        container.RegisterType(type, classType, buildKey.Name, new ContainerControlledLifetimeManager(), injectionMembers);
                    }
                }
            }
            else if (!type.IsInterface && context.PersistentPolicies.GetNoDefault<ILifetimePolicy>(buildKey, false) == null)
                context.PersistentPolicies.Set<ILifetimePolicy>(new ContainerControlledLifetimeManager(), buildKey);
        }

        public void PostBuildUp(IBuilderContext context)
        {
        }

        public void PostTearDown(IBuilderContext context)
        {
        }

        public void PreTearDown(IBuilderContext context)
        {
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(this, UnityBuildStage.Setup);
        }

        private static Type GetClassType(Type interfaceType)
        {
            Type classType = interfaceType.Assembly.GetType(interfaceType.Namespace + "." + interfaceType.Name.Substring(1));
            if (classType == null && interfaceType.Namespace != null && interfaceType.Namespace.Contains(".Interfaces"))
                classType = interfaceType.Assembly.GetType(interfaceType.Namespace.Replace(".Interfaces", string.Empty) + "." + interfaceType.Name.Substring(1));

            return classType;
        }
    }
}
