using Haystack.Analysis.ObjectModel;
using Haystack.Diagnostics.Configuration;
using Haystack.Diagnostics.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analysis
{
    public sealed class HaystackAnalysis
    {
        public HaystackAnalysis()
        {
        }

        public HaystackAnalysis(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
        {

        }

        public List<CodeCoverageAnalysis> CodeCoverageAnalysis { get; set; }

        public List<MethodCallTraceFileAnalysis> MethodCallTraceFiles { get; set; }

        public List<SourceControlRevision> SourceControlRevisions { get; set; }

        public List<HaystackMethod> HaystackMethods { get; set; }

        public HaystackMethodsWithRefactoring HaystackMethodsWithRefactoring { get; set; }

        public static void RunHaystackAnalysis(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
        {
            new MethodCallTraceAnalyzer().Analyze(passingConfiguration, failingConfiguration);
            new CodeCoverageAnalyzer().Analyze(passingConfiguration, failingConfiguration);
            new StaticAnalyzer().Analyze(passingConfiguration, failingConfiguration);
            new SourceControlAnalyzer().Analyze(passingConfiguration, failingConfiguration);
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
