using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analysis.SourceControl
{
    public sealed class LineChange : ILineChange
    {
        public LineChange(int lineNumber, IRevision revision)
        {
            LineNumber = lineNumber;
            Revision = revision;
        }

        public int LineNumber { get; private set; }

        public IRevision Revision { get; private set; }
    }
}
