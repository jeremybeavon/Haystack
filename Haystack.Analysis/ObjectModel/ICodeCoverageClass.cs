﻿using System.Collections.Generic;

namespace Haystack.Analysis.ObjectModel
{
    public interface ICodeCoverageClass
    {
        string NamespaceName { get; }

        string ClassName { get; }

        IEnumerable<ICodeCoverageMethod> Methods { get; }

        IEnumerable<ICodeCoverageNestedClass> NestedClasses { get; }

        ICodeCoverageClassFile File { get; }
    }
}
