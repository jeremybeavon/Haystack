using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analyzer.ObjectModel
{
    public interface ICodeCoverageMethod
    {
        int CodeCoverageMethodId { get; }

        string MethodName { get; }

        IEnumerable<ICodeCoverageMethodParameter> MethodParameters { get; }

        string ReturnType { get; }

        bool HasCodeCoverage { get; }

        IEnumerable<ICodeCoverageLine> Lines { get; }

        ICodeCoverageClass Class { get; }

        ICodeCoverageNestedClass NestedClass { get; }
    }
}
