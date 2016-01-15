using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Analyzer.ObjectModel
{
    internal sealed class HaystackMethodKey : IEquatable<HaystackMethodKey>, IEqualityComparer<HaystackMethodParameter>
    {
        public HaystackMethodKey([NotNull]string className, [NotNull]string methodName, [NotNull]IEnumerable<HaystackMethodParameter> methodParameters)
        {
            ClassName = className;
            MethodName = methodName;
            MethodParameters = methodParameters;
        }

        public string ClassName { get; private set; }

        public string MethodName { get; private set; }

        public IEnumerable<HaystackMethodParameter> MethodParameters { get; private set; }

        public override string ToString()
        {
            return ClassName + "." + MethodName + string.Join(",", MethodParameters.Select(ToString));
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(HaystackMethodKey other)
        {
            return other != null &&
                ClassName == other.ClassName &&
                MethodName == other.MethodName &&
                MethodParameters.SequenceEqual(other.MethodParameters, this);
        }

        public int GetHashCode(HaystackMethodParameter obj)
        {
            return ToString(obj).GetHashCode();
        }

        public bool Equals(HaystackMethodParameter x, HaystackMethodParameter y)
        {
            return x.Modifier == y.Modifier && x.ParameterType == y.ParameterType;
        }

        private static string ToString(HaystackMethodParameter parameter)
        {
            return parameter.Modifier + " " + parameter.ParameterType;
        }
    }
}
