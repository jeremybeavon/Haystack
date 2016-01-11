using System;
using Haystack.Diagnostics.Configuration;

namespace Haystack.Analysis
{
    public sealed class StaticAnalyzer : IHaystackAnalyzer
    {
        public void Analyze(IHaystackConfiguration passingConfiguration, IHaystackConfiguration failingConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}
