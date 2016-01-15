using CSharpDom.WithSyntax;
using Haystack.Analyzer.ObjectModel;
using Haystack.Diagnostics.CodeCoverage.OpenCover;
using Haystack.Diagnostics.Configuration;
using Haystack.Diagnostics.ObjectModel;
using HtmlAgilityPack;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Haystack.Analyzer
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

            InitializeCodeCoverageAnalysis(codeCoverageAnalysisList);
            return codeCoverageAnalysisList;
        }

        public void InitializeCodeCoverageAnalysis(List<CodeCoverageAnalysis> codeCoverageAnalysis)
        {

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
            classFile.Classes.AddRange(classes.Select(@class => LoadCodeCoverageClass(classFile, @class, namespaceName)));
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
                classFile.Classes.Add(LoadCodeCoverageClass(classFile, @struct, namespaceName));
            }
        }

        private CodeCoverageClass LoadCodeCoverageClass(CodeCoverageClassFile classFile, ClassWithSyntax @class, string namespaceName)
        {
            return LoadCodeCoverageClass(classFile, @class, namespaceName, @class.Name);
        }

        private CodeCoverageClass LoadCodeCoverageClass(CodeCoverageClassFile classFile, StructWithSyntax @struct, string namespaceName)
        {
            return LoadCodeCoverageClass(classFile, @struct, namespaceName, @struct.Name);
        }

        private CodeCoverageClass LoadCodeCoverageClass(
            CodeCoverageClassFile classFile,
            TypeWithSyntax type,
            string namespaceName,
            string typeName)
        {
            CodeCoverageClass codeCoverageClass = new CodeCoverageClass()
            {
                NamespaceName = namespaceName,
                ClassName = typeName,
                File = classFile
            };
            LoadCodeCoverageMethods(codeCoverageClass.Methods, type, method => method.Class = codeCoverageClass);
            codeCoverageClass.NestedClasses.AddRange(LoadCodeCoverageNestedClasses(codeCoverageClass, type.Classes));
            codeCoverageClass.NestedClasses.AddRange(LoadCodeCoverageNestedClasses(codeCoverageClass, type.Structs));
            return codeCoverageClass;
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            TypeWithSyntax type,
            Action<CodeCoverageMethod> initializeAction)
        {
            LoadCodeCoverageMethods(codeCoverageMethods, type.Methods, initializeAction);
            LoadCodeCoverageMethods(codeCoverageMethods, type.Constructors, initializeAction);
            LoadCodeCoverageMethods(codeCoverageMethods, type.Properties, initializeAction);
            LoadCodeCoverageMethods(codeCoverageMethods, type.Indexers, initializeAction);
            LoadCodeCoverageMethods(codeCoverageMethods, type.OperatorOverloads, initializeAction);
            LoadCodeCoverageMethods(codeCoverageMethods, type.ConversionOperators, initializeAction);
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            IEnumerable<MethodWithSyntax> methods,
            Action<CodeCoverageMethod> initializeAction)
        {
            foreach (MethodWithSyntax method in methods)
            {
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = method.Name,
                    ReturnType = method.Declaration.ReturnType.ToString()
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                initializeAction(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, method.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, method.StartLine, method.EndLine);
            }
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            IEnumerable<ConstructorWithSyntax> constructors,
            Action<CodeCoverageMethod> initializeAction)
        {
            foreach (ConstructorWithSyntax constructor in constructors)
            {
                bool isStatic = constructor.Declaration.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.StaticKeyword);
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = isStatic ? "cctor" : "ctor"
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                initializeAction(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, constructor.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, constructor.StartLine, constructor.EndLine);
            }
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            IEnumerable<PropertyWithSyntax> properties,
            Action<CodeCoverageMethod> initializeAction)
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
                    initializeAction(codeCoverageMethod);
                    FileLinePositionSpan lineSpan = accessor.GetLocation().GetLineSpan();
                    LoadCodeCoverageLines(codeCoverageMethod.Lines, lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Line);
                }
            }
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            IEnumerable<IndexerWithSyntax> indexers,
            Action<CodeCoverageMethod> initializeAction)
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
                    initializeAction(codeCoverageMethod);
                    FileLinePositionSpan lineSpan = accessor.GetLocation().GetLineSpan();
                    LoadCodeCoverageLines(codeCoverageMethod.Lines, lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Line);
                }
            }
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            IEnumerable<OperatorOverloadWithSyntax> operators,
            Action<CodeCoverageMethod> initializeAction)
        {
            foreach (OperatorOverloadWithSyntax @operator in operators)
            {
                CodeCoverageMethod codeCoverageMethod = new CodeCoverageMethod()
                {
                    MethodName = @operator.Declaration.OperatorToken.ToString(),
                    ReturnType = @operator.Declaration.ReturnType.ToString()
                };
                codeCoverageMethods.Add(codeCoverageMethod);
                initializeAction(codeCoverageMethod);
                LoadCodeCoverageMethodParameters(codeCoverageMethod.MethodParameters, @operator.Declaration.ParameterList);
                LoadCodeCoverageLines(codeCoverageMethod.Lines, @operator.StartLine, @operator.EndLine);
            }
        }

        private void LoadCodeCoverageMethods(
            List<CodeCoverageMethod> codeCoverageMethods,
            IEnumerable<ConversionOperatorWithSyntax> operators,
            Action<CodeCoverageMethod> initializeAction)
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
                initializeAction(codeCoverageMethod);
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
        
        private IEnumerable<CodeCoverageNestedClass> LoadCodeCoverageNestedClasses(
            CodeCoverageClass codeCoverageClass,
            IEnumerable<NestedClassWithSyntax> nestedClasses)
        {
            return nestedClasses.Select(nestedClass => LoadCodeCoverageNestedClass(codeCoverageClass, nestedClass));
        }

        private IEnumerable<CodeCoverageNestedClass> LoadCodeCoverageNestedClasses(
            CodeCoverageClass codeCoverageClass,
            IEnumerable<NestedStructWithSyntax> nestedClasses)
        {
            return nestedClasses.Select(nestedClass => LoadCodeCoverageNestedClass(codeCoverageClass, nestedClass));
        }

        private IEnumerable<CodeCoverageNestedClass> LoadCodeCoverageNestedClasses(
            CodeCoverageNestedClass codeCoverageClass,
            IEnumerable<NestedClassWithSyntax> nestedClasses)
        {
            return nestedClasses.Select(nestedClass => LoadCodeCoverageNestedClass(codeCoverageClass, nestedClass));
        }

        private IEnumerable<CodeCoverageNestedClass> LoadCodeCoverageNestedClasses(
            CodeCoverageNestedClass codeCoverageClass,
            IEnumerable<NestedStructWithSyntax> nestedClasses)
        {
            return nestedClasses.Select(nestedClass => LoadCodeCoverageNestedClass(codeCoverageClass, nestedClass));
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(
            CodeCoverageClass codeCoverageClass,
            NestedClassWithSyntax nestedClass)
        {
            CodeCoverageNestedClass codeCoverageNestedClass = LoadCodeCoverageNestedClass(nestedClass, nestedClass.Name);
            codeCoverageNestedClass.Class = codeCoverageClass;
            return codeCoverageNestedClass;
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(
            CodeCoverageClass codeCoverageClass,
            NestedStructWithSyntax nestedStruct)
        {
            CodeCoverageNestedClass codeCoverageNestedClass = LoadCodeCoverageNestedClass(nestedStruct, nestedStruct.Name);
            codeCoverageNestedClass.Class = codeCoverageClass;
            return codeCoverageNestedClass;
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(
            CodeCoverageNestedClass codeCoverageNestedClass,
            NestedClassWithSyntax nestedClass)
        {
            CodeCoverageNestedClass nestedCodeCoverageNestedClass = LoadCodeCoverageNestedClass(nestedClass, nestedClass.Name);
            nestedCodeCoverageNestedClass.NestedClass = codeCoverageNestedClass;
            return nestedCodeCoverageNestedClass;
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(
            CodeCoverageNestedClass codeCoverageNestedClass,
            NestedStructWithSyntax nestedStruct)
        {
            CodeCoverageNestedClass nestedCodeCoverageNestedClass = LoadCodeCoverageNestedClass(nestedStruct, nestedStruct.Name);
            nestedCodeCoverageNestedClass.NestedClass = codeCoverageNestedClass;
            return nestedCodeCoverageNestedClass;
        }

        private CodeCoverageNestedClass LoadCodeCoverageNestedClass(TypeWithSyntax type, string typeName)
        {
            CodeCoverageNestedClass codeCoverageNestedClass = new CodeCoverageNestedClass()
            {
                ClassName = typeName
            };
            LoadCodeCoverageMethods(codeCoverageNestedClass.Methods, type, method => method.NestedClass = codeCoverageNestedClass);
            codeCoverageNestedClass.NestedClasses.AddRange(LoadCodeCoverageNestedClasses(codeCoverageNestedClass, type.Classes));
            codeCoverageNestedClass.NestedClasses.AddRange(LoadCodeCoverageNestedClasses(codeCoverageNestedClass, type.Structs));
            return codeCoverageNestedClass;
        }

        private static string GetCodeCoverageOutputDirectory(IHaystackConfiguration configuration)
        {
            return Path.Combine(configuration.OutputDirectory, OpenCoverCoverage.OutputDirectoryName);
        }
    }
}
