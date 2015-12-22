using System;
using FluentAssertions;
using Haystack.Comparison.CodeCoverage;
using Haystack.Diagnostics.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodeBlacks.Tests
{
    [TestClass]
    public sealed class CodeCoverageComparisonTests
    {
        [TestMethod]
        public void Test_CompareDirectories_ShouldReturnOneFileDifferenceWhenOnlyOnePartialClassHasChanged()
        {
            string oldDirectory = "old";
            string newDirectory = "new";
            string oldFile = @"old\file.htm";
            string newFile = @"new\file.htm";
            Mock<IDirectoryReader> directoryReaderMock = new Mock<IDirectoryReader>(MockBehavior.Strict);
            directoryReaderMock
                .Setup(directoryReader => directoryReader.GetFiles(It.Is<string>(directory => directory == oldDirectory), It.IsAny<string>()))
                .Returns(new string[] { oldFile });
            directoryReaderMock
                .Setup(directoryReader => directoryReader.GetFiles(It.Is<string>(directory => directory == newDirectory), It.IsAny<string>()))
                .Returns(new string[] { newFile });
            Mock<IFileReader> fileReaderMock = new Mock<IFileReader>(MockBehavior.Strict);
            fileReaderMock.Setup(fileReader => fileReader.Exists(It.IsAny<string>())).Returns(true);
            fileReaderMock
                .Setup(fileReader => fileReader.ReadAllText(It.Is<string>(file => file == oldFile)))
                .Returns(CodeCoverageComparisonTestResources.RestSharp_RestClient_Old);
            fileReaderMock
                .Setup(fileReader => fileReader.ReadAllText(It.Is<string>(file => file == newFile)))
                .Returns(CodeCoverageComparisonTestResources.RestSharp_RestClient_New);
            new CodeCoverageComparison(directoryReaderMock.Object, fileReaderMock.Object)
                .CompareDirectories(oldDirectory, newDirectory)
                .Should().ContainSingle();
        }

        [TestMethod]
        public void Test_CompareFileContent_ShouldReturnNullIfOldFileContentIsNull()
        {
            CodeCoverageComparison.CompareFileContent(null, "test").Should().BeNull();
        }

        [TestMethod]
        public void Test_CompareFileContent_ShouldReturnNullIfNewFileContentIsNull()
        {
            CodeCoverageComparison.CompareFileContent("test", null).Should().BeNull();
        }

        [TestMethod]
        public void Test_CompareFileContent_ShouldReturnNullIfFilesAreIdentical()
        {
            CodeCoverageComparison.CompareFileContent("test", "test").Should().BeNull();
        }

        [TestMethod]
        public void Test_CompareFileContent_ShouldReturnNullIfBothFilesHaveNoCoverage()
        {
            const string matchText = "<tr><th>Coverage:</th><td>0%</td></tr>";
            CodeCoverageComparison.CompareFileContent("old" + matchText, "new" + matchText).Should().BeNull();
        }

        [TestMethod]
        public void Test_CompareFileContent_ShouldReturnOnNullIfThereIsNoCoverageChange()
        {
            CodeCoverageComparison.CompareFileContent(
                "OLD:" + CodeCoverageComparisonTestResources.RestSharp_RestClient_New,
                CodeCoverageComparisonTestResources.RestSharp_RestClient_New).Should().BeNull();
        }

        [TestMethod]
        public void Test_CompareFileContent_ShouldReturnOneFileDifferenceWhenOnlyOnePartialClassHasChanged()
        {
            CodeCoverageComparison.CompareFileContent(
                CodeCoverageComparisonTestResources.RestSharp_RestClient_Old,
                CodeCoverageComparisonTestResources.RestSharp_RestClient_New).Should().ContainSingle();
        }
    }
}
