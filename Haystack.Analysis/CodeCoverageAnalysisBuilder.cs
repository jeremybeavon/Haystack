using CSharpDom.WithSyntax;
using Haystack.Analysis.ObjectModel;
using Haystack.CodeCoverage.OpenCover;
using Haystack.Diagnostics.Configuration;
using HtmlAgilityPack;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Haystack.Analysis
{
    public sealed class CodeCoverageAnalysisBuilder
    {
        private CodeCoverageClassLines currentClassLines;

        public List<CodeCoverageAnalysis> LoadCodeCoverageAnalysis(
            IHaystackConfiguration passingConfiguration,
            IHaystackConfiguration failingConfiguration)
        {
            List<CodeCoverageAnalysis> codeCoverageAnalysisList = new List<CodeCoverageAnalysis>();
            string passingCodeCoverageDirectory = GetCodeCoverageOutputDirectory(passingConfiguration);
            string failingCodeCoverageDirectory = GetCodeCoverageOutputDirectory(failingConfiguration);
            List<string> failingCodeCoverageFiles = Directory.GetFiles(failingCodeCoverageDirectory, "*.htm").ToList();
            foreach (string passingCodeCoverageFile in Directory.GetFiles(passingCodeCoverageDirectory, "*.htm"))
            {
                CodeCoverageFile passingCoverageFile = LoadCodeCoverageFile(passingCodeCoverageFile);
                CodeCoverageFile failingCoverageFile = null;
                string failingCodeCoverageFile = Path.Combine(failingCodeCoverageDirectory, Path.GetFileName(passingCodeCoverageFile));
                if (File.Exists(failingCodeCoverageFile))
                {
                    failingCoverageFile = LoadCodeCoverageFile(failingCodeCoverageFile);
                    failingCodeCoverageFiles.Remove(failingCodeCoverageFile);
                }

                CodeCoverageAnalysis codeCoverageAnalysis = new CodeCoverageAnalysis()
                {
                    PassingCoverageFile = passingCoverageFile,
                    FailingCoverageFile = failingCoverageFile
                };
                codeCoverageAnalysisList.Add(codeCoverageAnalysis);
            }

            foreach (string failingCodeCoverageFile in failingCodeCoverageFiles)
            {
                CodeCoverageAnalysis codeCoverageAnalysis = new CodeCoverageAnalysis()
                {
                    FailingCoverageFile = LoadCodeCoverageFile(failingCodeCoverageFile)
                };
                codeCoverageAnalysisList.Add(codeCoverageAnalysis);
            }

            return codeCoverageAnalysisList;
        }

        private CodeCoverageFile LoadCodeCoverageFile(string codeCoverageFile)
        {
            CodeCoverageFile coverageFile = LoadCodeCoverageFile(LoadCodeCoverageLines(codeCoverageFile));
            coverageFile.FileName = codeCoverageFile;
            return coverageFile;
        }

        private List<CodeCoverageClassLines> LoadCodeCoverageLines(string codeCoverageFile)
        {
            List<CodeCoverageClassLines> codeCoverageClassLines = new List<CodeCoverageClassLines>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(codeCoverageFile);
            HtmlNode bodyNode = htmlDocument.DocumentNode.Element("html").Element("body");
            foreach (HtmlNode h2Node in bodyNode.Elements("h2"))
            {
                string fileName = h2Node.InnerText;
                CodeCoverageClassLines classLines = new CodeCoverageClassLines(fileName);
                codeCoverageClassLines.Add(classLines);
                HtmlNode tableNode = h2Node.NextSibling();
                foreach (HtmlNode rowNode in tableNode.Element("tbody").Elements("tr"))
                {
                    HtmlNode coverageNode = rowNode.Element("td").NextSibling();
                    HtmlNode lineNumberNode = coverageNode.NextSibling();
                    HtmlNode lineNode = lineNumberNode.NextSibling().NextSibling();
                    string coverageText = coverageNode.InnerText;
                    string lineNumberText = lineNumberNode.Element("code").InnerText;
                    string lineText = lineNode.Element("code").InnerText;
                    int coverage;
                    int.TryParse(coverageText, out coverage);
                    CodeCoverageLine codeCoverageLine = new CodeCoverageLine()
                    {
                        Coverage = coverage,
                        Line = lineText,
                        LineNumber = int.Parse(lineNumberText)
                    };
                    classLines.Lines.Add(codeCoverageLine);
                }
            }

            return codeCoverageClassLines;
        }

        private CodeCoverageFile LoadCodeCoverageFile(List<CodeCoverageClassLines> codeCoverageClassLines)
        {
            CodeCoverageFile codeCoverageFile = new CodeCoverageFile();
            foreach (CodeCoverageClassLines classLines in codeCoverageClassLines)
            {
                currentClassLines = classLines;
                CodeCoverageClassFile classFile = new CodeCoverageClassFile()
                {
                    FileName = classLines.FileName
                };
                codeCoverageFile.ClassFiles.Add(classFile);
                string text = classLines.Lines.Aggregate(new StringBuilder(), (builder, line) => builder.AppendLine(line.Line)).ToString();
                ProjectWithSyntax project = SolutionWithSyntax.CreateSolutionForText(text).Projects.First();
                project.Classes.GetAll().Then(classes => LoadCodeCoverageClasses(classFile, classes));
                project.Namespaces.GetAll().Then(namespaces => LoadCodeCoverageClasses(classFile, namespaces));
                project.Structs.GetAll().Then(structs => LoadCodeCoverageClasses(classFile, structs));
            }

            return codeCoverageFile;
        }

        private void LoadCodeCoverageClasses(
            CodeCoverageClassFile classFile,
            IEnumerable<ClassWithSyntax> classes,
            string namespaceName = null)
        {
            classFile.Classes.AddRange(classes.Select(@class => LoadCodeCoverageClass(@class, namespaceName)));
        }

        private void LoadCodeCoverageClasses(CodeCoverageClassFile classFile, IEnumerable<NamespaceWithSyntax> namespaces)
        {
            foreach (NamespaceWithSyntax @namespace in namespaces)
            {
                LoadCodeCoverageClasses(classFile, @namespace.Classes, @namespace.Name);
                LoadCodeCoverageClasses(classFile, @namespace.Structs, @namespace.Name);
            }
        }

        private void LoadCodeCoverageClasses(
            CodeCoverageClassFile classFile,
            IEnumerable<StructWithSyntax> structs,
            string namespaceName = null)
        {
            foreach (StructWithSyntax @struct in structs)
            {
                classFile.Classes.Add(LoadCodeCoverageClass(@struct, namespaceName));
            }
        }

        private CodeCoverageClass LoadCodeCoverageClass(ClassWithSyntax @class, string namespaceName)
        {
            return LoadCodeCoverageClass(@class, namespaceName, @class.Name);
        }

        private CodeCoverageClass LoadCodeCoverageClass(StructWithSyntax @struct, string namespaceName)
        {
            return LoadCodeCoverageClass(@struct, namespaceName, @struct.Name);
        }

        private CodeCoverageClass LoadCodeCoverageClass(TypeWithSyntax type, string namespaceName, string typeName)
        {
            CodeCoverageClass codeCoverageClass = new CodeCoverageClass()
            {
                NamespaceName = namespaceName,
                ClassName = typeName
            };
            LoadCodeCoverageMethods(codeCoverageClass.Methods, type);
            codeCoverageClass.NestedClasses.AddRange(type.Classes.Select(LoadCodeCoverageNestedClass));
            codeCoverageClass.NestedClasses.AddRange(type.Structs.Select(LoadCodeCoverageNestedClass));
            return codeCoverageClass;
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, TypeWithSyntax type)
        {
            foreach (MethodWithSyntax method in type.Methods)
            {
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = method.Name,
                    ReturnType = method.Declaration.ReturnType.ToString()
                };
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, method.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, method.Declaration);
            }
        }

        private void LoadCodeCoverageMethodParameters(
            List<CodeCoverageMethodParameter> codeCoverageMethodParameters,
            ParameterListSyntax parameters)
        {
        }

        private void LoadCodeCoverageLines(List<CodeCoverageLine> lines, CSharpSyntaxNode node)
        {
            FileLinePositionSpan position = node.GetLocation().GetLineSpan();
            int startLine = position.StartLinePosition.Line;
            int endLine = position.EndLinePosition.Line;
            for (int index = startLine; index <= endLine; index++)
            {
                lines.Add(currentClassLines.Lines[index]);
            }
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(NestedClassWithSyntax nestedClass)
        {
            return LoadCodeCoverageNestedClass(nestedClass, nestedClass.Name);
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(NestedStructWithSyntax nestedStruct)
        {
            return LoadCodeCoverageNestedClass(nestedStruct, nestedStruct.Name);
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(TypeWithSyntax type, string typeName)
        {
            CodeCoverageNestedClass codeCoverageNestedClass = new CodeCoverageNestedClass()
            {
                ClassName = typeName
            };
            LoadCodeCoverageMethods(codeCoverageNestedClass.Methods, type);
            codeCoverageNestedClass.NestedClasses.AddRange(type.Classes.Select(LoadCodeCoverageNestedClass));
            codeCoverageNestedClass.NestedClasses.AddRange(type.Structs.Select(LoadCodeCoverageNestedClass));
            return codeCoverageNestedClass;
        }

        private static string GetCodeCoverageOutputDirectory(IHaystackConfiguration configuration)
        {
            return Path.Combine(configuration.OutputDirectory, OpenCoverCoverage.OutputDirectoryName);
        }
    }
}
