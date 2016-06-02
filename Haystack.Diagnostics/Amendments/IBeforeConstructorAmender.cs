using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IBeforeConstructorAmender : IConstructorAmender
    {
        void BeforeConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters);
    }
}
