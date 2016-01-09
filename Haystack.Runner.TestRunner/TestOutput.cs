using System;
using System.IO;

namespace Haystack.Runner.TestRunner
{
    internal static class TestOutput
    {
        public static string OutputDirectory
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output"); }
        }

        public static string GetOutputFile<TTestIntegrationImplementation>()
        {
            return Path.Combine(OutputDirectory, typeof(TTestIntegrationImplementation).Name + ".txt");
        }
    }
}
