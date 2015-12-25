namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterPropertySetAmender : IPropertyAmender
    {
        void AfterPropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value);
    }
}
