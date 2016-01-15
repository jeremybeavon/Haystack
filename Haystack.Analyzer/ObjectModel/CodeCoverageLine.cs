namespace Haystack.Analyzer.ObjectModel
{
    public sealed class CodeCoverageLine : ICodeCoverageLine
    {
        public string Line { get; set; }

        public int LineNumber { get; set; }

        public int Coverage { get; set; }
    }
}
