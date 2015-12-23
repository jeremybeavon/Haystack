using Autofac;
using Autofac.Core;
using Haystack.Interception.Castle.Core;
using System;
using System.Linq;

namespace Haystack.Interception.Autofac
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
                    args.Instance = InstanceInterceptor.CreateInstance(interfaces[0], args.Instance, interfaces.Skip(1).ToArray());
                };
            }
        }
    }
}
