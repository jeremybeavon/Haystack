namespace Haystack.Diagnostics.SourceControl
{
    public interface ILineChange
    {
        int LineNumber { get; }
        
        string Revision { get; }
    }
}
