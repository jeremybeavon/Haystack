using System;
using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageClassFile : ICodeCoverageClassFile
    {
        public CodeCoverageClassFile()
        {
            Classes = new List<CodeCoverageClass>();
        }

        public string FileName { get; set; }

        public List<CodeCoverageClass> Classes { get; set; }

        IEnumerable<ICodeCoverageClass> ICodeCoverageClassFile.Classes
        {
            get { return Classes; }
        }
    }
}
