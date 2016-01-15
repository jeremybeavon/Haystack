using System;
using System.Collections.Generic;

namespace Haystack.Analyzer.ObjectModel
{
    public sealed class CodeCoverageFile : ICodeCoverageFile
    {
        public CodeCoverageFile()
        {
            ClassFiles = new List<CodeCoverageClassFile>();
        }

        public string FileName { get; set; }

        public List<CodeCoverageClassFile> ClassFiles { get; set; }

        IEnumerable<ICodeCoverageClassFile> ICodeCoverageFile.ClassFiles
        {
            get { return ClassFiles; }
        }
    }
}
