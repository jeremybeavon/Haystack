using System;

namespace Haystack.Diagnostics.SourceControl
{
    public interface ILineChange
    {
        int LineNumber { get; }
        
        IRevision Revision { get; } 
    }
}
