namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterMethodAmender : IMethodAmender
    {
        TReturnValue AfterMethod<TInstance, TReturnValue>(
            TInstance instance,
            string methodName,
            object[] parameters,
            TReturnValue returnValue);
    }
}
