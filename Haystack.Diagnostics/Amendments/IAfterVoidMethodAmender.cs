namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterVoidMethodAmender : IMethodAmender
    {
        void AfterMethod<TInstance>(TInstance instance, string methodName, object[] parameters);
    }
}
