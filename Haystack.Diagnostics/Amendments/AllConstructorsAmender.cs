using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public class AllConstructorsAmender : IConstructorAmender
    {
        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return true;
        }
    }
}
