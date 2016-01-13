using CSharpDom.WithSyntax;
using Haystack.Analysis.ObjectModel;
using Haystack.CodeCoverage.OpenCover;
using Haystack.Diagnostics.Configuration;
using Haystack.Diagnostics.ObjectModel;
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
            LoadCodeCoverageMethods(codeCoverageMethods, type.Methods);
            LoadCodeCoverageMethods(codeCoverageMethods, type.Constructors);
            LoadCodeCoverageMethods(codeCoverageMethods, type.Properties);
            LoadCodeCoverageMethods(codeCoverageMethods, type.Indexers);
            LoadCodeCoverageMethods(codeCoverageMethods, type.OperatorOverloads);
            LoadCodeCoverageMethods(codeCoverageMethods, type.ConversionOperators);
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, IEnumerable<MethodWithSyntax> methods)
        {
            foreach (MethodWithSyntax method in methods)
            {
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = method.Name,
                    ReturnType = method.Declaration.ReturnType.ToString()
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, method.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, method.StartLine, method.EndLine);
            }
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, IEnumerable<ConstructorWithSyntax> constructors)
        {
            foreach (ConstructorWithSyntax constructor in constructors)
            {
                bool isStatic = constructor.Declaration.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.StaticKeyword);
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = isStatic ? "cctor" : "ctor"
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, constructor.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, constructor.StartLine, constructor.EndLine);
            }
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, IEnumerable<PropertyWithSyntax> properties)
        {
            foreach (PropertyWithSyntax property in properties)
            {
                PropertyDeclarationSyntax propertyDeclaration = property.Declaration;
                foreach (AccessorDeclarationSyntax accessor in property.Declaration.AccessorList.Accessors)
                {
                    bool isGet = accessor.Keyword.Kind() == SyntaxKind.GetKeyword;
                    CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                    {
                        MethodName = (isGet ? "get_" : "set_") + property.Name,
                        ReturnType = isGet ? propertyDeclaration.Type.ToString() : "void"
                    };
                    if (!isGet)
                    {
                        CodeCoverageMethodParameter methodParameter = new CodeCoverageMethodParameter()
                        {
                            ParameterName = "value",
                            ParameterType = propertyDeclaration.Type.ToString()
                        };
                        codeCoverageMethod.MethodParameters.Add(methodParameter);
                    }
                    codeCoverageMethods.Add(codeCoverageMethod);
                    FileLinePositionSpan lineSpan = accessor.GetLocation().GetLineSpan();
                    LoadCodeCoverageLines(codeCoverageMethod.Lines, lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Line);
                }
            }
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, IEnumerable<IndexerWithSyntax> indexers)
        {
            foreach (IndexerWithSyntax indexer in indexers)
            {
                IndexerDeclarationSyntax indexerDeclaration = indexer.Declaration;
                foreach (AccessorDeclarationSyntax accessor in indexer.Declaration.AccessorList.Accessors)
                {
                    bool isGet = accessor.Keyword.Kind() == SyntaxKind.GetKeyword;
                    CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                    {
                        MethodName = isGet ? "get_Item" : "set_Item",
                        ReturnType = isGet ? indexerDeclaration.Type.ToString() : "void"
                    };
                    if (!isGet)
                    {
                        CodeCoverageMethodParameter methodParameter = new CodeCoverageMethodParameter()
                        {
                            ParameterName = "value",
                            ParameterType = indexerDeclaration.Type.ToString()
                        };
                        codeCoverageMethod.MethodParameters.Add(methodParameter);
                    }

                    LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, indexerDeclaration.ParameterList.Parameters);
                    codeCoverageMethods.Add(codeCoverageMethod);
                    FileLinePositionSpan lineSpan = accessor.GetLocation().GetLineSpan();
                    LoadCodeCoverageLines(codeCoverageMethod.Lines, lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Line);
                }
            }
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, IEnumerable<OperatorOverloadWithSyntax> operators)
        {
            foreach (OperatorOverloadWithSyntax @operator in operators)
            {
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = @operator.Declaration.OperatorToken.ToString(),
                    ReturnType = @operator.Declaration.ReturnType.ToString()
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, @operator.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, @operator.StartLine, @operator.EndLine);
            }
        }

        private void LoadCodeCoverageMethods(List<CodeCoverageMethod> codeCoverageMethods, IEnumerable<ConversionOperatorWithSyntax> operators)
        {
            foreach (ConversionOperatorWithSyntax @operator in operators)
            {
                ConversionOperatorDeclarationSyntax operatorDeclaration = @operator.Declaration;
                bool isImplicit = operatorDeclaration.ImplicitOrExplicitKeyword.Kind() == SyntaxKind.ImplicitKeyword;
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = isImplicit ? "op_Implicit" : "op_Explicit",
                    ReturnType = operatorDeclaration.Type.ToString()
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, @operatorDeclaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, @operator.StartLine, @operator.EndLine);
            }
        }

        private void LoadCodeCoverageMethodParameters(
            List<CodeCoverageMethodParameter> codeCoverageMethodParameters,
            ParameterListSyntax parameters)
        {
            LoadCodeCoverageMethodParameters(codeCoverageMethodParameters, parameters.Parameters);
        }

        private void LoadCodeCoverageMethodParameters(
            List<CodeCoverageMethodParameter> codeCoverageMethodParameters,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            foreach (ParameterSyntax parameter in parameters)
            {
                CodeCoverageMethodParameter codeCoverageMethodParameter = new CodeCoverageMethodParameter()
                {
                    ParameterName = parameter.Identifier.ValueText,
                    ParameterType = parameter.Type.ToString()
                };
                codeCoverageMethodParameters.Add(codeCoverageMethodParameter);
                if (parameter.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.RefKeyword))
                {
                    codeCoverageMethodParameter.Modifier = ParameterModifier.Ref;
                }
                else if (parameter.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.OutKeyword))
                {
                    codeCoverageMethodParameter.Modifier = ParameterModifier.Out;
                }
            }
        }

        private void LoadCodeCoverageLines(List<CodeCoverageLine> lines, int startLine, int endLine)
        {
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
