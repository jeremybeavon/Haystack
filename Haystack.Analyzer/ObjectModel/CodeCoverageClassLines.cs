using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class CodeCoverageClassLines
    {
        public CodeCoverageClassLines(string fileName)
        {
            FileName = fileName;
            Lines = new List<CodeCoverageLine>();
        }

        public string FileName { get; private set; }

        public List<CodeCoverageLine> Lines { get; private set; }
    }
}
