using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IConstructorAmender
    {
        bool AmendConstructor(ConstructorInfo constructor);
    }
}
