using Haystack.Analysis.ObjectModel;
using Haystack.Diagnostics.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Analysis
{
    public sealed class HaystackMethodBuilder
    {
        private const bool isPassingMethodCallTrace = true;
        private const bool isNotPassingMethodCallTrace = false;
        private readonly IDictionary<HaystackMethodKey, HaystackMethod> haystackMethods;
        private HaystackAnalysis haystackAnalysis;
        private bool isProcessingPassingCodeCoverage;

        public HaystackMethodBuilder()
        {
            haystackMethods = new Dictionary<HaystackMethodKey, HaystackMethod>();
        }

        public void LoadHaystackMethods(HaystackAnalysis haystackAnalysis)
        {
            this.haystackAnalysis = haystackAnalysis;
            foreach (CodeCoverageAnalysis codeCoverageAnalysis in haystackAnalysis.CodeCoverageAnalysis)
            {
                ProcessCodeCoverageAnalysis(codeCoverageAnalysis);
            }

            foreach (MethodCallTraceFileAnalysis methodCallTraceFileAnalysis in haystackAnalysis.MethodCallTraceFileAnalysis)
            {
                ProcessMethodCallTraceFileAnalysis(methodCallTraceFileAnalysis);
            }
        }

        private void ProcessCodeCoverageAnalysis(CodeCoverageAnalysis codeCoverageAnalysis)
        {
            if (codeCoverageAnalysis.PassingCoverageFile != null)
            {
                isProcessingPassingCodeCoverage = true;
                ProcessCodeCoverageClassFiles(codeCoverageAnalysis.PassingCoverageFile.ClassFiles);
                isProcessingPassingCodeCoverage = false;
            }

            if (codeCoverageAnalysis.FailingCoverageFile != null)
            {
                ProcessCodeCoverageClassFiles(codeCoverageAnalysis.FailingCoverageFile.ClassFiles);
            }
        }

        private void ProcessCodeCoverageClassFiles(IEnumerable<CodeCoverageClassFile> codeCoverageClassFiles)
        {
            foreach (CodeCoverageClass codeCoverageClass in codeCoverageClassFiles.SelectMany(classFile => classFile.Classes))
            {
                string className = codeCoverageClass.NamespaceName + "." + codeCoverageClass.ClassName;
                ProcessCodeCoverageMethods(className, codeCoverageClass.Methods);
                ProcessCodeCoverageNestedClasses(className, codeCoverageClass.NestedClasses);
            }
        }

        private void ProcessCodeCoverageNestedClasses(string className, IEnumerable<CodeCoverageNestedClass> codeCoverageNestedClasses)
        {
            foreach (CodeCoverageNestedClass codeCoverageNestedClass in codeCoverageNestedClasses)
            {
                string nestedClassName = className + "+" + codeCoverageNestedClass.ClassName;
                ProcessCodeCoverageMethods(nestedClassName, codeCoverageNestedClass.Methods);
                ProcessCodeCoverageNestedClasses(nestedClassName, codeCoverageNestedClass.NestedClasses);
            }
        }

        private void ProcessCodeCoverageMethods(string className, IEnumerable<CodeCoverageMethod> codeCoverageMethods)
        {
            foreach (CodeCoverageMethod codeCoverageMethod in codeCoverageMethods)
            {
                string methodName = codeCoverageMethod.MethodName;
                List<HaystackMethodParameter> haystackMethodParameters = GetHaystackMethodParameters(codeCoverageMethod);
                HaystackMethodKey haystackMethodKey = new HaystackMethodKey(className, methodName, haystackMethodParameters);
                HaystackMethod haystackMethod;
                if (!haystackMethods.TryGetValue(haystackMethodKey, out haystackMethod))
                {
                    haystackMethod = CreateAndAddHaystackMethod(haystackMethodKey);
                }

                if (isProcessingPassingCodeCoverage)
                {
                    haystackMethod.PassingCodeCoverageMethod = codeCoverageMethod;
                    haystackMethod.PassingMethodCoverageMethodId = codeCoverageMethod.CodeCoverageMethodId;
                }
                else
                {
                    haystackMethod.FailingCodeCoverageMethod = codeCoverageMethod;
                    haystackMethod.FailingMethodCoverageMethodId = codeCoverageMethod.CodeCoverageMethodId;
                }
            }
        }

        private void ProcessMethodCallTraceFileAnalysis(MethodCallTraceFileAnalysis methodCallTraceFileAnalysis)
        {
            if (methodCallTraceFileAnalysis.PassingMethodCallTrace != null)
            {
                ProcessMethodCallTrace(methodCallTraceFileAnalysis.PassingMethodCallTrace, isPassingMethodCallTrace);
            }

            if (methodCallTraceFileAnalysis.FailingMethodCallTrace != null)
            {
                ProcessMethodCallTrace(methodCallTraceFileAnalysis.FailingMethodCallTrace, isNotPassingMethodCallTrace);
            }
        }

        private void ProcessMethodCallTrace(MethodCallTrace methodCallTrace, bool isPassingMethodCallTrace)
        {
            foreach (MethodCall methodCall in methodCallTrace.MethodCallThreads.SelectMany(thread => thread.MethodCalls))
            {
                string className = methodCall.DeclaringType.TypeName;
                string methodName = methodCall.MethodName;
                List<HaystackMethodParameter> haystackMethodParameters = GetHaystackMethodParameters(methodCall);
                HaystackMethodKey haystackMethodKey = new HaystackMethodKey(className, methodName, haystackMethodParameters);
                HaystackMethod haystackMethod;
                if (!haystackMethods.TryGetValue(haystackMethodKey, out haystackMethod))
                {
                    haystackMethod = CreateAndAddHaystackMethod(haystackMethodKey);
                }

                methodCall.ResetMethodCallId();
                if (isPassingMethodCallTrace)
                {
                    haystackMethod.PassingMethodCalls.Add(methodCall);
                    haystackMethod.PassingMethodCallIds.Add(methodCall.MethodCallId);
                }
                else
                {
                    haystackMethod.FailingMethodCalls.Add(methodCall);
                    haystackMethod.FailingMethodCallIds.Add(methodCall.MethodCallId);
                }
            }
        }

        private HaystackMethod CreateAndAddHaystackMethod(HaystackMethodKey haystackMethodKey)
        {
            HaystackMethod haystackMethod = new HaystackMethod()
            {
                ClassName = haystackMethodKey.ClassName,
                MethodName = haystackMethodKey.MethodName,
                MethodParameters = haystackMethodKey.MethodParameters.ToList()
            };
            haystackAnalysis.HaystackMethods.Add(haystackMethod);
            haystackMethods.Add(haystackMethodKey, haystackMethod);
            return haystackMethod;
        }

        private static List<HaystackMethodParameter> GetHaystackMethodParameters(CodeCoverageMethod codeCoverageMethod)
        {
            return codeCoverageMethod.MethodParameters.Select(GetHaystackMethodParameter).ToList();
        }

        private static List<HaystackMethodParameter> GetHaystackMethodParameters(MethodCall methodCall)
        {
            return methodCall.Parameters.Select(GetHaystackMethodParameter).ToList();
        }

        private static HaystackMethodParameter GetHaystackMethodParameter(CodeCoverageMethodParameter methodParameter)
        {
            return new HaystackMethodParameter()
            {
                Modifier = methodParameter.Modifier,
                ParameterType = methodParameter.ParameterType,
                ParameterName = methodParameter.ParameterName
            };
        }

        private static HaystackMethodParameter GetHaystackMethodParameter(MethodParameter methodParameter)
        {
            return new HaystackMethodParameter()
            {
                Modifier = methodParameter.Modifier,
                ParameterType = methodParameter.ParameterType.TypeName,
                ParameterName = methodParameter.ParameterName
            };
        }
    }
}
