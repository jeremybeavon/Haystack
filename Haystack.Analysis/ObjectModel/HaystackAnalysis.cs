using Haystack.Diagnostics;
using Haystack.Diagnostics.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class HaystackAnalysis : IHaystackAnalysis
    {
        public HaystackAnalysis()
        {
            CodeCoverageAnalysis = new List<CodeCoverageAnalysis>();
            MethodCallTraceFiles = new List<MethodCallTraceFileAnalysis>();
            SourceControlRevisions = new List<SourceControlRevision>();
            HaystackMethods = new List<HaystackMethod>();
            HaystackMethodsWithRefactoring = new HaystackMethodsWithRefactoring();
        }

        public HaystackAnalysis(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
            : this()
        {
            LoadCodeCoverageAnalysis(passingConfiguration, failingConfiguration);
            LoadMethodCallFileAnalysis(passingConfiguration, failingConfiguration);
            LoadSourceControlRevisions(passingConfiguration, failingConfiguration);
            LoadHaystackMethods();
            LoadHaystackMethodsWithRefactoring();
        }

        public List<CodeCoverageAnalysis> CodeCoverageAnalysis { get; set; }

        public List<MethodCallTraceFileAnalysis> MethodCallTraceFiles { get; set; }

        public List<SourceControlRevision> SourceControlRevisions { get; set; }

        public List<HaystackMethod> HaystackMethods { get; set; }

        public HaystackMethodsWithRefactoring HaystackMethodsWithRefactoring { get; set; }

        IEnumerable<ICodeCoverageAnalysis> IHaystackAnalysis.CodeCoverageAnalysis
        {
            get { return CodeCoverageAnalysis; }
        }

        public IEnumerable<IMethodCallTraceFileAnalysis> MethodCallTraceFileAnalysis
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

        private void LoadCodeCoverageAnalysis(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
        {
            new CodeCoverageAnalysisBuilder().LoadCodeCoverageAnalysis(passingConfiguration, failingConfiguration);
        }

        private void LoadMethodCallFileAnalysis(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
        {
            List<string> failingMethodCallTraceFiles = GetMethodCallTraceFiles(failingConfiguration).ToList();
            foreach (string passingMethodCallTraceFile in GetMethodCallTraceFiles(passingConfiguration))
            {
                string fileName = Path.GetFileName(passingMethodCallTraceFile);
                MethodCallTraceFileAnalysis methodCallTraceFileAnalysis = new MethodCallTraceFileAnalysis()
                {
                    FileName = fileName,
                    PassingMethodCallTrace = new MethodCallTraceProvider().Load(passingMethodCallTraceFile)
                };
                MethodCallTraceFiles.Add(methodCallTraceFileAnalysis);
                string failingMethodCallTraceFile = Path.Combine(failingConfiguration.OutputDirectory, fileName);
                if (File.Exists(failingMethodCallTraceFile))
                {
                    methodCallTraceFileAnalysis.FailingMethodCallTrace = new MethodCallTraceProvider().Load(failingMethodCallTraceFile);
                    failingMethodCallTraceFiles.Remove(failingMethodCallTraceFile);
                }
            }

            foreach (string failingMethodCallTraceFile in failingMethodCallTraceFiles)
            {
                MethodCallTraceFileAnalysis methodCallTraceFileAnalysis = new MethodCallTraceFileAnalysis()
                {
                    FileName = Path.Combine(failingMethodCallTraceFile),
                    PassingMethodCallTrace = new MethodCallTraceProvider().Load(failingMethodCallTraceFile)
                };
                MethodCallTraceFiles.Add(methodCallTraceFileAnalysis);
            }
        }

        private void LoadSourceControlRevisions(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
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
        
        private static string[] GetMethodCallTraceFiles(IHaystackConfiguration configuration)
        {
            return Directory.GetFiles(configuration.OutputDirectory, "haystack.callTrace.*");
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
