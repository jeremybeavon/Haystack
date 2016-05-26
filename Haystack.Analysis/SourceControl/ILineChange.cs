using System;

namespace Haystack.Analysis.SourceControl
{
    public interface ILineChange
    {
        int LineNumber { get; }
        
        IRevision Revision { get; } 
    }
}
