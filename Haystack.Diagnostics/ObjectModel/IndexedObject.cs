namespace Haystack.Diagnostics.ObjectModel
{
    internal sealed class IndexedObject<T>
    {
        public IndexedObject()
        {
        }

        public IndexedObject(T @object, int index)
        {
            Object = @object;
            Index = index;
        }

        public int Index { get; set; }

        public T Object { get; set; }
    }
}
