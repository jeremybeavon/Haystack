using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analysis.ObjectModel
{
    public interface ICodeCoverageMethodParameter
    {
        ParameterModifier Modifier { get; }

        string ParameterType { get; }

        string ParameterName { get; }
    }
}
