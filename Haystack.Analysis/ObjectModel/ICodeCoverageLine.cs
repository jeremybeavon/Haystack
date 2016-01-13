﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analysis.ObjectModel
{
    public interface ICodeCoverageLine
    {
        string Line { get; }

        int LineNumber { get; }

        int Coverage { get; }
    }
}
