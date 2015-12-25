namespace Haystack.Diagnostics.Amendments
{
    public interface IBeforeConstructorAmender : IConstructorAmender
    {
        void BeforeConstructor<TInstance>(TInstance instance, object[] parameters);
    }
}
