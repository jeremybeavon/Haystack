namespace Haystack.Diagnostics.SourceControl
{
    public interface ISourceControlSelector
    {
        ISourceControlProvider GetSourceControlProvider(string fileName);
    }
}
