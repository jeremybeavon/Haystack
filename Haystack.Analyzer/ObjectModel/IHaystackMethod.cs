using Haystack.Diagnostics.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analyzer.ObjectModel
{
    public interface IHaystackMethod
    {
        string ClassName { get; }

        string MethodName { get; }

        IEnumerable<IHaystackMethodParameter> MethodParameters { get; }

        CodeCoverageMethodDifferenceType CodeCoverageDifference { get; }

        ICodeCoverageMethod PassingCodeCoverageMethod { get; }

        ICodeCoverageMethod FailingCodeCoverageMethod { get; }

        IEnumerable<IMethodCall> PassingMethodCalls { get; }

        IEnumerable<IMethodCall> FailingMethodCalls { get; }

        IEnumerable<ISourceControlLineChange> SourceControlLineChanges { get; }
    }
}
