using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Haystack.Diagnostics.Interception.Castle.Core;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Diagnostics.Interception.Castle.Windsor
{
    public sealed class HaystackInterceptor : AbstractFacility
    {
        private readonly InterceptorReference interceptor;

        public HaystackInterceptor()
        {
            interceptor = new InterceptorReference(typeof(InstanceInterceptor));
        }

        public static void SetUp(IWindsorContainer container)
        {
            container.Register(Component.For<InstanceInterceptor>().LifestyleSingleton());
            container.AddFacility(new HaystackInterceptor());
        }

        protected override void Init()
        {
            UpdateRegisteredComponents(Kernel.GraphNodes.OfType<ComponentModel>(), new HashSet<ComponentModel>());
            Kernel.ComponentRegistered += ComponentRegistered;
        }

        private void ComponentRegistered(string key, IHandler handler)
        {
            if (handler.ComponentModel.Implementation != typeof(InstanceInterceptor))
            {
                handler.ComponentModel.Interceptors.Add(interceptor);
            }
        }

        private void UpdateRegisteredComponents(IEnumerable<ComponentModel> components, ISet<ComponentModel> set)
        {
            foreach (ComponentModel component in components)
            {
                if (component.Implementation != typeof(InstanceInterceptor) && set.Add(component))
                {
                    component.Interceptors.Add(interceptor);
                    UpdateRegisteredComponents(component.Dependencies.OfType<ComponentModel>(), set);
                }
            }
        }
    }
}
