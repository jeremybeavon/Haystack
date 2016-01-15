using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class HaystackMethodParameter : IHaystackMethodParameter
    {
        public ParameterModifier Modifier { get; set; }

        public string ParameterType { get; set; }

        public string ParameterName { get; set; }
    }
}
