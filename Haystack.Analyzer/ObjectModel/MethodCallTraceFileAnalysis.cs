using System;
using Haystack.Diagnostics.ObjectModel;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class MethodCallTraceFileAnalysis : IMethodCallTraceFileAnalysis
    {
        public string FileName { get; set; }

        public MethodCallTrace PassingMethodCallTrace { get; set; }

        public MethodCallTrace FailingMethodCallTrace { get; set; }

        IMethodCallTrace IMethodCallTraceFileAnalysis.PassingMethodCallTrace
        {
            get { return PassingMethodCallTrace; }
        }

        IMethodCallTrace IMethodCallTraceFileAnalysis.FailingMethodCallTrace
        {
            get { return FailingMethodCallTrace; }
        }
    }
}
