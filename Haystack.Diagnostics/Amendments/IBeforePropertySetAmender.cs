namespace Haystack.Diagnostics.Amendments
{
    public interface IBeforePropertySetAmender : IPropertyAmender
    {
        TProperty BeforePropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value);
    }
}
