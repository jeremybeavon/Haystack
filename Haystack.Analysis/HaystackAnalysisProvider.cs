using Haystack.Analysis.ObjectModel;
using Haystack.Diagnostics;
using Haystack.Diagnostics.ObjectModel;
using MsgPack.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Haystack.Analysis
{
    public static class HaystackAnalysisProvider
    {
        public static void Save(string fileName, HaystackAnalysis haystackAnalysis)
        {
            using (Stream stream = File.Create(fileName))
            {
                SerializationContext.Default.GetSerializer<HaystackAnalysis>().Pack(stream, haystackAnalysis);
            }
        }

        public static HaystackAnalysis Load(string fileName)
        {
            HaystackAnalysis haystackAnalysis;
            using (Stream stream = File.OpenRead(fileName))
            {
                haystackAnalysis = SerializationContext.Default.GetSerializer<HaystackAnalysis>().Unpack(stream);
            }

            Initialize(haystackAnalysis);
            return haystackAnalysis;
        }

        public static void Initialize(HaystackAnalysis haystackAnalysis)
        {
            IDictionary<int, CodeCoverageMethod> codeCoverageMethods = new Dictionary<int, CodeCoverageMethod>();
            foreach (CodeCoverageAnalysis codeCoverageAnalysis in haystackAnalysis.CodeCoverageAnalysis)
            {
                Initialize(codeCoverageAnalysis.PassingCoverageFile, codeCoverageMethods);
                Initialize(codeCoverageAnalysis.FailingCoverageFile, codeCoverageMethods);
            }

            IDictionary<int, MethodCall> methodCalls = new Dictionary<int, MethodCall>();
            foreach (MethodCallTraceFileAnalysis methodCallTraceFileAnalysis in haystackAnalysis.MethodCallTraceFileAnalysis)
            {
                Initialize(methodCallTraceFileAnalysis.PassingMethodCallTrace, methodCalls);
                Initialize(methodCallTraceFileAnalysis.FailingMethodCallTrace, methodCalls);
            }

            Initialize(haystackAnalysis.HaystackMethods, codeCoverageMethods, methodCalls);
            Initialize(haystackAnalysis.HaystackMethodsWithRefactoring, haystackAnalysis.HaystackMethods);
        }

        private static void Initialize(CodeCoverageFile codeCoverageFile, IDictionary<int, CodeCoverageMethod> methods)
        {
            foreach (CodeCoverageClass codeCoverageClass in codeCoverageFile.ClassFiles.SelectMany(@class => @class.Classes))
            {
                foreach (CodeCoverageMethod codeCoverageMethod in codeCoverageClass.Methods)
                {
                    codeCoverageMethod.Class = codeCoverageClass;
                    methods.Add(codeCoverageMethod.CodeCoverageMethodId, codeCoverageMethod);
                }

                foreach (CodeCoverageNestedClass codeCoverageNestedClass in codeCoverageClass.NestedClasses)
                {
                    codeCoverageNestedClass.Class = codeCoverageClass;
                    Initialize(codeCoverageNestedClass, methods);
                }
            }
        }

        private static void Initialize(
            CodeCoverageNestedClass codeCoverageNestedClass,
            IDictionary<int, CodeCoverageMethod> methods)
        {
            foreach (CodeCoverageMethod codeCoverageMethod in codeCoverageNestedClass.Methods)
            {
                codeCoverageMethod.NestedClass = codeCoverageNestedClass;
                methods.Add(codeCoverageMethod.CodeCoverageMethodId, codeCoverageMethod);
            }

            foreach (CodeCoverageNestedClass nestedCodeCoverageNestedClass in codeCoverageNestedClass.NestedClasses)
            {
                nestedCodeCoverageNestedClass.NestedClass = codeCoverageNestedClass;
                Initialize(nestedCodeCoverageNestedClass, methods);
            }
        }

        private static void Initialize(MethodCallTrace methodCallTrace, IDictionary<int, MethodCall> methodCalls)
        {
            if (methodCallTrace == null)
            {
                return;
            }

            MethodCallTraceProvider.Initialize(methodCallTrace);
            foreach (MethodCallThreadTrace thread in methodCallTrace.MethodCallThreads)
            {
                Initialize(thread.MethodCalls, methodCalls);
            }
        }

        private static void Initialize(IEnumerable<MethodCall> methodCalls, IDictionary<int, MethodCall> methodCallMap)
        {
            foreach (MethodCall methodCall in methodCalls)
            {
                methodCallMap.Add(methodCall.MethodCallId, methodCall);
                Initialize(methodCall.MethodCalls, methodCallMap);
            }
        }

        private static void Initialize(
            IEnumerable<HaystackMethod> haystackMethods,
            IDictionary<int, CodeCoverageMethod> codeCoverageMethods,
            IDictionary<int, MethodCall> methodCalls)
        {
            foreach (HaystackMethod haystackMethod in haystackMethods)
            {
                if (haystackMethod.PassingMethodCoverageMethodId != null)
                {
                    haystackMethod.PassingCodeCoverageMethod = codeCoverageMethods[haystackMethod.PassingMethodCoverageMethodId.Value];
                }

                if (haystackMethod.FailingMethodCoverageMethodId != null)
                {
                    haystackMethod.FailingCodeCoverageMethod = codeCoverageMethods[haystackMethod.FailingMethodCoverageMethodId.Value];
                }

                haystackMethod.PassingMethodCalls = GetMethodCalls(haystackMethod.PassingMethodCallIds, methodCalls);
                haystackMethod.FailingMethodCalls = GetMethodCalls(haystackMethod.FailingMethodCallIds, methodCalls);
            }
        }

        private static void Initialize(HaystackMethodsWithRefactoring haystackMethodsWithRefactoring, List<HaystackMethod> haystackMethods)
        {
            haystackMethodsWithRefactoring.NonRefactoredMethods = haystackMethodsWithRefactoring.NonRefactoredMethodIndexes
                .Select(index => haystackMethods[index])
                .ToList();
            foreach (RefactoredMethod refactoredMethod in haystackMethodsWithRefactoring.RefactoredMethods)
            {
                refactoredMethod.PassingMethod = haystackMethods[refactoredMethod.PassingMethodIndex];
                refactoredMethod.FailingMethod = haystackMethods[refactoredMethod.FailingMethodIndex];
            }
        }

        private static List<MethodCall> GetMethodCalls(IEnumerable<int> methodCallIds, IDictionary<int, MethodCall> methodCalls)
        {
            return methodCallIds.Select(methodCallId => methodCalls[methodCallId]).ToList();
        }
    }
}
