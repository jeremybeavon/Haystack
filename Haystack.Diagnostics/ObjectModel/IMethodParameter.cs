namespace Haystack.Diagnostics.ObjectModel
{
    public interface IMethodParameter
    {
        IObjectType ParameterType { get; }

        string ParameterName { get; }

        ParameterModifier Modifier { get; }

        IValue Value { get; }

        IValue OutputValue { get; }
    }
}
