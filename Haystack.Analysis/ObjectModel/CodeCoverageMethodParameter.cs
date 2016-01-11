using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageMethodParameter
    {
        public ParameterModifier Modifier { get; set; }

        public string ParameterType { get; set; }

        public string ParameterName { get; set; }
    }
}
