using Haystack.Interception.Castle.Core;
using Microsoft.Practices.ObjectBuilder2;
using System;

namespace Haystack.Interception.Unity
{
    public sealed class HaystackInterceptionBuilderStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            Type interfaceType = context.OriginalBuildKey.Type;
            if (interfaceType.IsInterface)
            {
                context.Existing = InstanceInterceptor.CreateInstance(interfaceType, context.Existing);
            }
        }
    }
}
