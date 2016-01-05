using System;

namespace Haystack.Diagnostics
{
    public abstract class CrossDomainObject : MarshalByRefObject
    {
        public sealed override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
