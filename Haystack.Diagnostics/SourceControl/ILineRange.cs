namespace Haystack.Diagnostics.SourceControl
{
    public interface ILineRange
    {
        int StartLine { get; }

        int EndLine { get; }
    }
}
