using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analyzer.ObjectModel
{
    public interface IMethodCallTraceFileAnalysis
    {
        string FileName { get; }

        IMethodCallTrace PassingMethodCallTrace { get; }

        IMethodCallTrace FailingMethodCallTrace { get; }
    }
}
