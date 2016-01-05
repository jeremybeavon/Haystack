using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.Windsor;
using Haystack.Interception.Castle.Core;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Interception.Castle.Windsor
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
            container.AddFacility(new HaystackInterceptor());
        }

        protected override void Init()
        {
            UpdateRegisteredComponents(Kernel.GraphNodes.OfType<ComponentModel>(), new HashSet<ComponentModel>());
            Kernel.ComponentRegistered += ComponentRegistered;
        }

        private void ComponentRegistered(string key, IHandler handler)
        {
            handler.ComponentModel.Interceptors.Add(interceptor);
        }

        private void UpdateRegisteredComponents(IEnumerable<ComponentModel> components, ISet<ComponentModel> set)
        {
            foreach (ComponentModel component in components)
            {
                if (set.Add(component))
                {
                    component.Interceptors.Add(interceptor);
                    UpdateRegisteredComponents(component.Dependencies.OfType<ComponentModel>(), set);
                }
            }
        }
    }
}
