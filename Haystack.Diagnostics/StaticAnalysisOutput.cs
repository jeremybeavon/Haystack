using System.Collections.Generic;

namespace Haystack.Diagnostics
{
    public sealed class StaticAnalysisOutput
    {
        public string Description { get; set; }

        public List<string> Before { get; set; }

        public List<string> After { get; set; }
    }
}
