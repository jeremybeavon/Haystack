using System;

namespace Haystack.Core
{
    public abstract class CrossDomainObject : MarshalByRefObject
    {
        public sealed override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
