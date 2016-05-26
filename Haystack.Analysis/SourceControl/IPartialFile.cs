using System.Collections.Generic;

namespace Haystack.Analysis.SourceControl
{
    public interface IPartialFile
    {
        string FileName { get; }

        IReadOnlyCollection<ILineRange> LineRanges { get; }
    }
}
