using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public interface ICodeCoverageClassFile
    {
        string FileName { get; }

        IEnumerable<ICodeCoverageClass> Classes { get; }
    }
}
