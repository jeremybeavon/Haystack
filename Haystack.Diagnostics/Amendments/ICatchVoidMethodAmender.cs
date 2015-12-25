namespace Haystack.Diagnostics.Amendments
{
    public interface ICatchVoidMethodAmender : IMethodAmender
    {
        void CatchMethod<TInstance, TException>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters);
    }
}
