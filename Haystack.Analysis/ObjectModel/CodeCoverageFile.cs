using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageFile
    {
        public CodeCoverageFile()
        {
            Classes = new List<CodeCoverageClass>();
        }

        public string FileName { get; set; }

        public List<CodeCoverageClass> Classes { get; set; }
    }
}
