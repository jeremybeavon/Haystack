using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class MethodCallTraceFileAnalysis
    {
        public string FileName { get; set; }

        public MethodCallTrace PassingMethodCallTrace { get; set; }

        public MethodCallTrace FailingMethodCallTrace { get; set; }
    }
}
