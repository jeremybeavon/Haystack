using Haystack.Diagnostics.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haystack.Analysis
{
    internal sealed class MethodCallThreadTraceText
    {
        private readonly string text;
        private readonly List<MethodCall> methodCalls;
        private readonly List<int> methodCallIndexes;

        public MethodCallThreadTraceText(MethodCallThreadTrace thread)
        {
            methodCalls = new List<MethodCall>();
            methodCallIndexes = new List<int>();
            text = ToString(thread);
        }

        public string Text
        {
            get { return text; }
        }

        public MethodCall GetMethodCall(int lineIndex)
        {
            int methodCallIndex = methodCallIndexes.BinarySearch(lineIndex);
            if (methodCallIndex < 0)
            {
                methodCallIndex = ~methodCallIndex - 1;
            }

            return methodCalls[methodCallIndex];
        }

        private static string GetMethodDefinition(MethodCall methodCall)
        {
            StringBuilder textBuilder = new StringBuilder();
            textBuilder.Append(methodCall.DeclaringType.TypeName);
            textBuilder.Append(".");
            textBuilder.Append(methodCall.MethodName);
            textBuilder.Append("(");
            textBuilder.Append(string.Join(", ", methodCall.Parameters.Select(GetParameterDefinition)));
            textBuilder.Append(") : ");
            textBuilder.Append(methodCall.ReturnType.TypeName);
            return textBuilder.ToString();
        }

        private static string GetParameterDefinition(MethodParameter parameter)
        {
            return (parameter.Modifier == ParameterModifier.None ?
                string.Empty :
                (parameter.Modifier.ToString().ToLower() + " ")) + parameter.ParameterType.TypeName;
        }

        private string ToString(MethodCallThreadTrace thread)
        {
            TextBuilder textBuilder = new TextBuilder();
            foreach (MethodCall methodCall in thread.MethodCalls)
            {
                ToString(methodCall, textBuilder);
            }

            return textBuilder.ToString();
        }

        private void ToString(MethodCall methodCall, TextBuilder textBuilder)
        {
            methodCalls.Add(methodCall);
            methodCallIndexes.Add(textBuilder.LineCount);
            string methodDescription = GetMethodDefinition(methodCall);
            textBuilder.AppendLine("Starting: " + methodDescription);
            foreach (MethodParameter parameter in methodCall.Parameters)
            {
                if (parameter.Modifier != ParameterModifier.Out)
                {
                    textBuilder.AppendLine("Parameter " + parameter.ParameterName + " = " + parameter.Value.RawValue);
                }

                if (parameter.Modifier != ParameterModifier.None)
                {
                    textBuilder.AppendLine("Parameter output " + parameter.ParameterName + " = " + parameter.Value.RawValue);
                }
            }

            textBuilder.AppendLine("Return value = " + methodCall.ReturnValue.RawValue);
            foreach (MethodCall childMethodCall in methodCall.MethodCalls)
            {
                ToString(childMethodCall, textBuilder);
            }

            textBuilder.AppendLine("Ending: " + methodDescription);
        }
        
        private sealed class TextBuilder
        {
            private readonly StringBuilder textBuilder;
            private int lineCount;

            public TextBuilder()
            {
                textBuilder = new StringBuilder();
            }

            public int LineCount
            {
                get { return lineCount; }
            }

            public void AppendLine(string text)
            {
                textBuilder.AppendLine(text);
                lineCount++;
            }

            public override string ToString()
            {
                return textBuilder.ToString();
            }
        }
    }
}
