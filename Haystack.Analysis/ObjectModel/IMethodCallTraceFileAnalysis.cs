using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analysis.ObjectModel
{
    public interface IMethodCallTraceFileAnalysis
    {
        string FileName { get; }

        IMethodCallTrace PassingMethodCallTrace { get; }

        IMethodCallTrace FailingMethodCallTrace { get; }
    }
}
