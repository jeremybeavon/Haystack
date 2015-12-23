using Haystack.Interception.Castle.Core;
using StructureMap;
using StructureMap.Building.Interception;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;

namespace Haystack.Interception.StructureMap
{
    public class HaystackInterceptionPolicy : IInterceptorPolicy
    {
        public string Description
        {
            get { return "Haystack"; }
        }

        public static void SetUp(Container container)
        {
            container.Model.Pipeline.Policies.Add(new HaystackInterceptionPolicy());
        }

        public IEnumerable<IInterceptor> DetermineInterceptors(Type pluginType, Instance instance)
        {
            if (pluginType.IsInterface)
                yield return new FuncInterceptor<object>(target => InstanceInterceptor.CreateInstance(pluginType, target));
        }
    }
}
