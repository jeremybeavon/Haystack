using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analyzer.ObjectModel
{
    public interface ISourceControlRevision
    {
        string Revision { get; }

        string Author { get; }

        DateTime ChangeDate { get; }
    }
}
