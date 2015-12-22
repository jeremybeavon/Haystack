using Afterthought;
using Haystack.Diagnostics;

namespace Haystack.Interception.Afterthought
{
    public class PropertyAmender<T> : Amendment<T, T>
    {
        public PropertyAmender()
        {
            Properties
                .BeforeGet(PropertyDiagnostics<T>.BeforePropertyGet)
                .AfterGet(PropertyDiagnostics<T>.AfterPropertyGet)
                .BeforeSet(PropertyDiagnostics<T>.BeforePropertySet)
                .AfterSet(PropertyDiagnostics<T>.AfterPropertySet);
        }
    }
}
