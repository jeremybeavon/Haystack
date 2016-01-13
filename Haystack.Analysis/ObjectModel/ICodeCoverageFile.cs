using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public interface ICodeCoverageFile
    {
        string FileName { get; }

        IEnumerable<ICodeCoverageClassFile> ClassFiles { get; }
    }
}
