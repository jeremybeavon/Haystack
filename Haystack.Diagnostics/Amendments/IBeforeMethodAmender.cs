namespace Haystack.Diagnostics.Amendments
{
    public interface IBeforeMethodAmender : IMethodAmender
    {
        void BeforeMethod<TInstance>(TInstance instance, string methodName, object[] parameters);
    }
}
