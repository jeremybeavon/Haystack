namespace Haystack.Diagnostics.Amendments
{
    public interface IFinallyMethodAmender : IMethodAmender
    {
        void Finally<TInstance>(TInstance instance, string methodName, object[] parameters);
    }
}
