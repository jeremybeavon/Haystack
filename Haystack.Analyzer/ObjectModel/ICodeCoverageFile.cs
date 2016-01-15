using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public interface ICodeCoverageFile
    {
        string FileName { get; }

        IEnumerable<ICodeCoverageClassFile> ClassFiles { get; }
    }
}
