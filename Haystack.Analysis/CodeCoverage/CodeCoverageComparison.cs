using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Haystack.Diagnostics.IO;

namespace Haystack.Analysis.CodeCoverage
{
    public sealed class CodeCoverageComparison
    {
        private readonly IDirectoryReader directoryReader;
        private readonly IFileReader fileReader;

        public CodeCoverageComparison()
            : this(new DirectoryReader(), new FileReader())
        {
        }

        public CodeCoverageComparison(IDirectoryReader directoryReader, IFileReader fileReader)
        {
            this.directoryReader = directoryReader;
            this.fileReader = fileReader;
        }

        public IEnumerable<FileDifferences> CompareDirectories(string oldDirectory, string newDirectory)
        {
            List<string> fileNames = new List<string>();
            List<FileDifferences> differences = new List<FileDifferences>();
            foreach (string newFile in directoryReader.GetFiles(newDirectory, "*.htm"))
            {
                string newFileName = Path.GetFileName(newFile);
                string oldFile = Path.Combine(oldDirectory, newFileName);
                fileNames.Add(oldFile);
                if (newFileName == "index.htm")
                {
                    continue;
                }

                string oldFileContent = fileReader.Exists(oldFile) ? fileReader.ReadAllText(oldFile) : string.Empty;
                string newFileContent = fileReader.ReadAllText(newFile);
                IEnumerable<FileDifferences> fileDifferences = CompareFileContent(oldFileContent, newFileContent);
                if (fileDifferences != null)
                {
                    differences.AddRange(fileDifferences);
                }
            }

            AddDeletedFileComparisons(oldDirectory, newDirectory, fileNames, differences);
            return differences;
        }

        public IEnumerable<FileDifferences> CompareFiles(string oldFile, string newFile)
        {
            return CompareFileContent(fileReader.ReadAllText(oldFile), fileReader.ReadAllText(newFile));
        }

        public static IEnumerable<FileDifferences> CompareFileContent(string oldFileContent, string newFileContent)
        {
            if (oldFileContent == null || newFileContent == null)
            {
                Trace.WriteLine("One of the file content is null");
                return null;
            }

            if (oldFileContent == newFileContent)
            {
                Trace.WriteLine("Files are identical");
                return null;
            }

            const string matchText = "<tr><th>Coverage:</th><td>0%</td></tr>";
            if (oldFileContent.Contains(matchText) && newFileContent.Contains(matchText))
            {
                Trace.WriteLine("Both files have no coverage.");
                return null;
            }
            
            oldFileContent = RemoveLineNumbers(oldFileContent);
            newFileContent = RemoveLineNumbers(newFileContent);
            SideBySideDiffBuilder sideBySideDiffer = new SideBySideDiffBuilder(new Differ());
            SideBySideDiffModel diff = sideBySideDiffer.BuildDiffModel(oldFileContent, newFileContent);
            DiffPiece[] newLines = diff.NewText.Lines.Where(DoesLineHaveDifferentCoverage).ToArray();
            if (!diff.OldText.Lines.Where(DoesLineHaveDifferentCoverage).Concat(newLines).Any())
            {
                Trace.WriteLine("No changes to code coverage");
                return null;
            }

            return AnalyzeDiff(diff, newLines);
        }

        private static IEnumerable<FileDifferences> AnalyzeDiff(SideBySideDiffModel diff, DiffPiece[] newLines)
        {
            IList<Range> oldClassIndexRanges = FindClassIndexRanges(diff.OldText.Lines);
            IList<Range> newClassIndexRanges = FindClassIndexRanges(diff.NewText.Lines);
            if (oldClassIndexRanges.Count != newClassIndexRanges.Count)
            {
                throw new NotImplementedException();
            }

            IList<Range> classIndexRanges = FindClassIndexRanges(oldClassIndexRanges, newClassIndexRanges);
            return BuildFileDifferences(diff, classIndexRanges);
        }
        
        private static IList<Range> FindClassIndexRanges(IEnumerable<DiffPiece> lines)
        {
            IList<Range> classIndexRanges = new List<Range>();
            int index = -1;
            foreach (DiffPiece line in lines)
            {
                index++;
                if (line.Text != null && line.Text.StartsWith("<h2"))
                {
                    classIndexRanges.Add(new Range(index));
                }
                else if (line.Text == "</table>" && classIndexRanges.Count != 0)
                {
                    classIndexRanges.Last().End = index;
                }
            }

            return classIndexRanges;
        }

        private static IList<Range> FindClassIndexRanges(IList<Range> oldClassIndexRanges, IList<Range> newClassIndexRanges)
        {
            IList<Range> classIndexRanges = new List<Range>();
            for (int index = 0; index < oldClassIndexRanges.Count; index++)
            {
                Range oldIndexRange = oldClassIndexRanges[index];
                Range newIndexRange = newClassIndexRanges[index];
                int newStartIndex = Math.Min(oldIndexRange.Start, newIndexRange.Start);
                int newEndIndex = Math.Max(oldIndexRange.End, newIndexRange.End);
                classIndexRanges.Add(new Range(newStartIndex, newEndIndex));
            }

            return classIndexRanges;
        }

        private static IEnumerable<FileDifferences> BuildFileDifferences(SideBySideDiffModel diff, IEnumerable<Range> classIndexRanges)
        {
            IList<FileDifferences> fileDifferences = new List<FileDifferences>();
            using (IEnumerator<Range> classIndexRange = classIndexRanges.GetEnumerator())
            {
                classIndexRange.MoveNext();
                FileDifferences currentDifference = null;
                bool doesFileHaveCoverageDifferences = false;
                int count = diff.NewText.Lines.Count;
                for (int index = 0; index < count; index++)
                {    
                    if (index < classIndexRange.Current.Start)
                    {
                        continue;
                    }

                    if (index == classIndexRange.Current.Start)
                    {
                        string line = diff.OldText.Lines[index].Text ?? diff.NewText.Lines[index].Text;
                        string fileName = Regex.Match(line, "<h2[^>]*>(?<FileName>[^<]+)").Groups["FileName"].Value;
                        currentDifference = new FileDifferences(Path.GetFileName(fileName), new SideBySideDiffModel());
                    }
                    else
                    {
                        currentDifference.Differences.OldText.Lines.Add(diff.OldText.Lines[index]);
                        currentDifference.Differences.NewText.Lines.Add(diff.NewText.Lines[index]);
                        doesFileHaveCoverageDifferences = doesFileHaveCoverageDifferences ||
                            diff.OldText.Lines[index].Type != ChangeType.Unchanged ||
                            diff.NewText.Lines[index].Type != ChangeType.Unchanged;
                        if (index == classIndexRange.Current.End)
                        {
                            if (doesFileHaveCoverageDifferences)
                            {
                                fileDifferences.Add(currentDifference);
                            }

                            if (!classIndexRange.MoveNext())
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return fileDifferences;
        }
        
        private static string RemoveLineNumbers(string fileContent)
        {
            const string lineNumberPattern = @"(?<Prefix>(?:<td[^<]+</td>){2})(?:<td class=""rightmargin right""><code>\d+</code></td>)";
            return Regex.Replace(fileContent, lineNumberPattern, "${Prefix}");
        }

        private static bool DoesLineHaveDifferentCoverage(DiffPiece line)
        {
            bool isDifferent = line.Type != ChangeType.Unchanged && line.Text != null && Regex.IsMatch(line.Text, @"'VC':\s*'\d+'");
            if (isDifferent)
            {
                line.Text = Regex.Replace(line.Text, "^<tr ", "<tr class=\"danger\" ");
            }
            else
            {
                line.Type = ChangeType.Unchanged;
            }

            return isDifferent;
        }

        private void AddDeletedFileComparisons(
            string oldDiectory,
            string newDirectory,
            IEnumerable<string> existingFiles,
            List<FileDifferences> differences)
        {
            foreach (string oldFile in directoryReader.GetFiles(oldDiectory, "*.htm").Except(existingFiles))
            {
                CompareFileContent(fileReader.ReadAllText(oldFile), string.Empty);
            }
        }
    }
}
