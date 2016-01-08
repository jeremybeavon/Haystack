using System;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class HaystackBeforeMethodAmender : HaystackMethodAmender, IBeforeMethodAmender
    {
        public HaystackBeforeMethodAmender(IMethodAmender amender)
            : base(amender)
        {
        }

        public void BeforeMethod<TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
