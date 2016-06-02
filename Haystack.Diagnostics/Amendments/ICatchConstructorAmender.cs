using System.Reflection;

namespace Haystack.Diagnostics.Amendments
{
    public interface ICatchConstructorAmender : IConstructorAmender
    {
        void CatchConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters);
    }
}
