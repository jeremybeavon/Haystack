namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterConstructorAmender : IConstructorAmender
    {
        void AfterConstructor<TInstance>(TInstance instance, object[] parameters);
    }
}
