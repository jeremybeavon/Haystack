using System;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class CodeCoverageAnalysis : ICodeCoverageAnalysis
    {
        public CodeCoverageFile PassingCoverageFile { get; set; }

        public CodeCoverageFile FailingCoverageFile { get; set; }

        ICodeCoverageFile ICodeCoverageAnalysis.PassingCoverageFile
        {
            get { return PassingCoverageFile; }
        }

        ICodeCoverageFile ICodeCoverageAnalysis.FailingCoverageFile
        {
            get { return FailingCoverageFile; }
        }
    }
}
