namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageLine
    {
        public string Line { get; set; }

        public int LineNumber { get; set; }

        public int Coverage { get; set; }
    }
}
