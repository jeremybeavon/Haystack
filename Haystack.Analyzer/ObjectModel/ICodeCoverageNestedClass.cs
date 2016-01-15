using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public interface ICodeCoverageNestedClass
    {
        string ClassName { get; }

        IEnumerable<ICodeCoverageMethod> Methods { get; }

        IEnumerable<ICodeCoverageNestedClass> NestedClasses { get; }

        ICodeCoverageClass Class { get; }
    }
}
