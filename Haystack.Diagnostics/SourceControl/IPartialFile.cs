using System.Collections.Generic;

namespace Haystack.Diagnostics.SourceControl
{
    public interface IPartialFile
    {
        string FileName { get; }

        IReadOnlyCollection<ILineRange> LineRanges { get; }
    }
}
