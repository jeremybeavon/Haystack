namespace Haystack.Analysis.SourceControl
{
    public interface ILineRange
    {
        int StartLine { get; }

        int EndLine { get; }
    }
}
