namespace Haystack.Analysis.SourceControl
{
    public interface ISourceControlSelector
    {
        ISourceControlProvider GetSourceControlProvider(string fileName);
    }
}
