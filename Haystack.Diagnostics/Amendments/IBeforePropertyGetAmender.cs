namespace Haystack.Diagnostics.Amendments
{
    public interface IBeforePropertyGetAmender : IPropertyAmender
    {
        void BeforePropertyGet<TInstance>(TInstance instance, string propertyName);
    }
}
