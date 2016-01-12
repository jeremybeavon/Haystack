using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class CodeCoverageFile
    {
        public CodeCoverageFile()
        {
            ClassFiles = new List<CodeCoverageClassFile>();
        }

        public string FileName { get; set; }

        public List<CodeCoverageClassFile> ClassFiles { get; set; }
    }
}
