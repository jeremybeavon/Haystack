using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterConstructorAmender : IConstructorAmender
    {
        void AfterConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters);
    }
}
