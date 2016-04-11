using System;

namespace Haystack.Diagnostics.Amendments
{
    public interface IPropertyAmender
    {
        bool AmendProperty(Type type, string propertyName);
    }
}
