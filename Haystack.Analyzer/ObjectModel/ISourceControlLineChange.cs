namespace Haystack.Analyzer.ObjectModel
{
    public interface ISourceControlLineChange
    {
        int Line { get; }

        ISourceControlRevision Revision { get; }
    }
}
