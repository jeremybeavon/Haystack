namespace Haystack.Diagnostics.Amendments
{
    public interface IAfterPropertyGetAmender : IPropertyAmender
    {
        TProperty AfterPropertyGet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value);
    }
}
