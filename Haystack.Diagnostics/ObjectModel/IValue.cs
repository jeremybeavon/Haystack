namespace Haystack.Diagnostics.ObjectModel
{
    public interface IValue
    {
        string RawValue { get; }

        IObjectInstance Object { get; }
    }
}
