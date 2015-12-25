namespace Haystack.Diagnostics.Amendments
{
    public interface ICatchConstructorAmender : IConstructorAmender
    {
        void CatchConstructor<TInstance>(TInstance instance, object[] parameters);
    }
}
