using Haystack.Diagnostics;
using Haystack.Analysis.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class HaystackAnalysis : IHaystackAnalysis
    {
        public HaystackAnalysis()
        {
            CodeCoverageAnalysis = new List<CodeCoverageAnalysis>();
            MethodCallTraceFileAnalysis = new List<MethodCallTraceFileAnalysis>();
            SourceControlRevisions = new List<SourceControlRevision>();
            HaystackMethods = new List<HaystackMethod>();
            HaystackMethodsWithRefactoring = new HaystackMethodsWithRefactoring();
        }

        public HaystackAnalysis(IHaystackAnalysisConfiguration configuration)
            : this()
        {
            LoadCodeCoverageAnalysis(configuration);
            LoadMethodCallFileAnalysis(configuration);
            LoadSourceControlRevisions(configuration);
            LoadHaystackMethods();
            LoadHaystackMethodsWithRefactoring();
        }

        public List<CodeCoverageAnalysis> CodeCoverageAnalysis { get; set; }

        public List<MethodCallTraceFileAnalysis> MethodCallTraceFileAnalysis { get; set; }

        public List<SourceControlRevision> SourceControlRevisions { get; set; }

        public List<HaystackMethod> HaystackMethods { get; set; }

        public HaystackMethodsWithRefactoring HaystackMethodsWithRefactoring { get; set; }

        IEnumerable<ICodeCoverageAnalysis> IHaystackAnalysis.CodeCoverageAnalysis
        {
            get { return CodeCoverageAnalysis; }
        }

        IEnumerable<IMethodCallTraceFileAnalysis> IHaystackAnalysis.MethodCallTraceFileAnalysis
        {
            get { return MethodCallTraceFileAnalysis; }
        }

        IEnumerable<ISourceControlRevision> IHaystackAnalysis.SourceControlRevisions
        {
            get { return SourceControlRevisions; }
        }

        IEnumerable<IHaystackMethod> IHaystackAnalysis.HaystackMethods
        {
            get { return HaystackMethods; }
        }

        IHaystackMethodsWithRefactoring IHaystackAnalysis.HaystackMethodsWithRefactoring
        {
            get { return HaystackMethodsWithRefactoring; }
        }

        private void LoadCodeCoverageAnalysis(IHaystackAnalysisConfiguration configuration)
        {
            foreach (ICodeCoverageConfiguration codeCoverageConfiguration in configuration.CodeCoverage)
            {
                LoadCodeCoverageAnalysis(configuration, codeCoverageConfiguration);
            }
        }

        private void LoadCodeCoverageAnalysis(
            IHaystackAnalysisConfiguration configuration,
            ICodeCoverageConfiguration codeCoverageConfiguration)
        {
            string providerAssembly = Path.Combine(
                    configuration.HaystackAnalysisDirectory,
                    codeCoverageConfiguration.CodeCoverageFramework,
                    codeCoverageConfiguration.CodeCoverageAnalysisProviderAssembly);
            Assembly assembly = Assembly.LoadFrom(providerAssembly);
            CodeCoverageAnalysisProviderAttribute attribute = assembly.GetCustomAttribute<CodeCoverageAnalysisProviderAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException("Missing CodeCoverageAnalysisProviderAttribute");
            }

            ICodeCoverageAnalysisProvider provider = Activator.CreateInstance(attribute.CodeCoverageAnalysisProviderType) as ICodeCoverageAnalysisProvider;
            if (provider == null)
            {
                throw new InvalidOperationException(attribute.CodeCoverageAnalysisProviderType + " does not implement ICodeCoverageAnalysisProvider.");
            }

            CodeCoverageAnalysis.AddRange(provider.AnalyzeCodeCoverage(configuration));
        }

        private void LoadMethodCallFileAnalysis(IHaystackAnalysisConfiguration configuration)
        {
            List<string> failingMethodCallTraceFiles = GetMethodCallTraceFiles(configuration.FailingTestOutputDirectory).ToList();
            foreach (string passingMethodCallTraceFile in GetMethodCallTraceFiles(configuration.PassingTestOutputDirectory))
            {
                string fileName = Path.GetFileName(passingMethodCallTraceFile);
                MethodCallTraceFileAnalysis methodCallTraceFileAnalysis = new MethodCallTraceFileAnalysis()
                {
                    FileName = fileName,
                    PassingMethodCallTrace = MethodCallTraceProvider.Load(passingMethodCallTraceFile)
                };
                MethodCallTraceFileAnalysis.Add(methodCallTraceFileAnalysis);
                string failingMethodCallTraceFile = Path.Combine(configuration.FailingTestOutputDirectory, fileName);
                if (File.Exists(failingMethodCallTraceFile))
                {
                    methodCallTraceFileAnalysis.FailingMethodCallTrace = MethodCallTraceProvider.Load(failingMethodCallTraceFile);
                    failingMethodCallTraceFiles.Remove(failingMethodCallTraceFile);
                }
            }

            foreach (string failingMethodCallTraceFile in failingMethodCallTraceFiles)
            {
                MethodCallTraceFileAnalysis methodCallTraceFileAnalysis = new MethodCallTraceFileAnalysis()
                {
                    FileName = Path.Combine(failingMethodCallTraceFile),
                    PassingMethodCallTrace = MethodCallTraceProvider.Load(failingMethodCallTraceFile)
                };
                MethodCallTraceFileAnalysis.Add(methodCallTraceFileAnalysis);
            }
        }

        private void LoadSourceControlRevisions(IHaystackAnalysisConfiguration configuration)
        {
        }

        private void LoadHaystackMethods()
        {
            new HaystackMethodBuilder().LoadHaystackMethods(this); 
        }

        private void LoadHaystackMethodsWithRefactoring()
        {
            int index = 0;
            foreach (HaystackMethod method in HaystackMethods)
            {
                HaystackMethodsWithRefactoring.NonRefactoredMethodIndexes.Add(index);
                HaystackMethodsWithRefactoring.NonRefactoredMethods.Add(method);
                index++;
            }
        }
        
        private static string[] GetMethodCallTraceFiles(string outputDirectory)
        {
            return Directory.GetFiles(outputDirectory, "haystack.callTrace.*");
        }
    }
}

/*
    HaystackAnalysis
        CodeCoverageAnalysis[] CodeCoverageAnalysis
        MethodCallTraceFileAnalysis[] MethodCallTraceFileAnalysis
        SourceControlRevision[] SourceControlRevisions
        HaystackMethod[] HaystackMethods
        HaystackMethodsWithRefactoring HaystackMethodsWithRefactoring
    
    HaystackMethodsWithRefactoring
        RefactoredMethod[] RefactoredMethods
        HaystackMethod[] NonRefactoredMethods    

    RefactoredMethod : HaystackMethod
        HaystackMethod PassingMethod
        HaystackMethod FailingMethod
        MethodRefactorTypes RefactorTypes

    [Flags]
    MethodRefactorTypes
        MethodRename,
        TypeRename,
        ParameterAdded,
        ParameterRemoved,
        ParameterChanged,
        ReturnTypeChanged

    HaystackMethod
        string MethodName
        string TypeName
        HaystackMethodParameter[] MethodParameters
        string ReturnType
        CodeCoverageMethodDifferenceType CodeCoverageDifference
        CodeCoverageMethod PassingCodeCoverageMethod
        CoveCoverageMethod FailingCodeCoverageMethod
        MethodCall[] PassingMethodCalls
        MethodCall[] FailingMethodCalls
        SourceControlChange[] SourceControlChanges
        
    HaystackMethodParameter
        ParameterModifier Modifier
        string ParameterType
        string ParameterName

    MethodCallTraceFileAnalysis
        string FileName
        MethodCallTrace PassingMethodCallTrace
        MethodCallTrace FailingMethodCallTrace

    SourceControlLineChange
        int Line
        SourceControlRevision Revision

    SourceControlRevision
        string Revision
        string Author
        DateTime CommitDate

    CodeCoverageLine[] GetCoverageLines(string fileName); (Use HtmlAgilityPack)
    CodeCoverageFile GetCoverageFile(CodeCoverageLine[] lines); (Uses CSharpDom)
    CodeCoverageAnalysis GetCodeCoverageAnalysis(CodeCoverageFile passingCoverageFile, CodeCoverageFile failingCoverageFile);

    CodeCoverageAnalysis
        CodeCoverageFile PassingCoverageFile
        CodeCoverageFile FailingCoverageFile

    CodeCoverageMethodDifferenceType
        NoData,
        NoCoverage,
        IdenticalCodeCoverage,
        CodeCoverageDifferentAndTextIdentical,
        CodeCoverageDifferentAndTextDifferent,
        PassingCodeCoverageMethodMissing,
        FailingCodeCoverageMethodMissing

    CodeCoverageFile
        string FileName
        CodeCoverageClass[] Classes

    CodeCoverageClass
        string NamespaceName
        string ClassName
        CodeCoverageMethod[] Methods
        CodeCoverageNestedClass NestedClasses[]
        CodeCoverageFile File

    CodeCoverageNestedClass
        string ClassName
        CodeCoverageMethod[] Methods
        CodeCoverageNestedClass NestedClass
        CodeCoverageClass Class

    CodeCoverageMethod
        string MethodName
        CodeCoverageMethodParameter[] MethodParameters
        string ReturnType
        CodeCoverageLine[] Lines
        bool HasCodeCoverage
        CodeCoverageNestedClass NestedClass
        CodeCoverageClass Class
                
    CodeCoverageMethodParameter
        ParameterModifier Modifier,
        string ParameterType,
        string ParameterName
                
    CodeCoverageLine
        string Line
        int LineNumber
        int Coverage
*/
