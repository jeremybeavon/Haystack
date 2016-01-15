using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analyzer.ObjectModel
{
    public interface ICodeCoverageMethodParameter
    {
        ParameterModifier Modifier { get; }

        string ParameterType { get; }

        string ParameterName { get; }
    }
}
