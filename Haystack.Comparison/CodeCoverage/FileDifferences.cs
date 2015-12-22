﻿using DiffPlex.DiffBuilder.Model;

namespace Haystack.Comparison.CodeCoverage
{
    public sealed class FileDifferences
    {
        public FileDifferences(string fileName, SideBySideDiffModel differences)
        {
            FileName = fileName;
            Differences = differences;
        }

        public string FileName { get; private set; }

        public SideBySideDiffModel Differences { get; private set; }
    }
}
