using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public interface ICodeCoverageClassFile
    {
        string FileName { get; }

        IEnumerable<ICodeCoverageClass> Classes { get; }
    }
}
