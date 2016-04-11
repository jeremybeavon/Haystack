using System.Collections.Generic;
using System.Linq;

namespace Haystack.Core
{
    public static class ListExtensions
    {
        public static void Insert<T>(this IList<T> list, int index, IEnumerable<T> itemsToAdd)
        {
            foreach (T item in itemsToAdd.Reverse())
            {
                list.Insert(index, item);
            }
        }
    }
}
