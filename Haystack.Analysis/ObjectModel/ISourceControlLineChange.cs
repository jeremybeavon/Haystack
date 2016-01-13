namespace Haystack.Analysis.ObjectModel
{
    public interface ISourceControlLineChange
    {
        int Line { get; }

        ISourceControlRevision Revision { get; }
    }
}
