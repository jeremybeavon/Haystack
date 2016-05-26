using System.Collections.Generic;

namespace Haystack.Analysis.SourceControl
{
    public sealed class PartialFile : IPartialFile
    {
        public PartialFile(string fileName, IReadOnlyCollection<ILineRange> lineRanges)
        {
            FileName = fileName;
            LineRanges = lineRanges;
        }

        public string FileName { get; private set; }

        public IReadOnlyCollection<ILineRange> LineRanges { get; private set; }
    }
}
