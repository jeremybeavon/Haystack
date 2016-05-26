using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analysis.ObjectModel
{
    public interface IHaystackMethodParameter
    {
        ParameterModifier Modifier { get; }

        string ParameterType { get; }

        string ParameterName { get; }
    }
}
