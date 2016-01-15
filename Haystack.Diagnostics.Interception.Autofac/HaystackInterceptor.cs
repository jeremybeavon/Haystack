using Autofac;
using Autofac.Core;
using Haystack.Diagnostics.Interception.Castle.Core;
using System;
using System.Linq;

namespace Haystack.Diagnostics.Interception.Autofac
{
    public static class HaystackInterceptor
    {
        public static void SetUp(ContainerBuilder builder)
        {
            builder.RegisterCallback(SetUp);
        }

        public static void SetUp(IContainer container)
        {
            SetUp(container.ComponentRegistry);
        }

        private static void SetUp(IComponentRegistry registry)
        {
            foreach (var registration in registry.Registrations)
            {
                registration.Activating += (sender, args) =>
                {
                    Type[] interfaces = args.Instance.GetType().GetInterfaces().Where(type => type.IsVisible).ToArray();
                    if (interfaces.Length == 0)
                    {
                        return;
                    }

                    Type[] additionalInterfaces = interfaces.Skip(1).ToArray();
                    args.Instance = InstanceInterceptor.CreateInstance(interfaces[0], args.Instance, additionalInterfaces);
                };
            }
        }
    }
}
