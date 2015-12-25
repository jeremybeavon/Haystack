namespace Haystack.Diagnostics.Amendments
{
    public interface ICatchMethodAmender : IMethodAmender
    {
        TReturnValue CatchMethod<TInstance, TException, TReturnValue>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters);
    }
}
