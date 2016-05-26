namespace Haystack.Analysis.SourceControl
{
    public sealed class LineRange : ILineRange
    {
        public LineRange(int startLine, int endLine)
        {
            StartLine = startLine;
            EndLine = endLine;
        }

        public int StartLine { get; private set; }

        public int EndLine { get; private set; }
    }
}
