using Haystack.Core;
using System.IO;
using System.Reflection;
using System.Text;

namespace Haystack.Diagnostics.CodeCoverage.OpenCover
{
    public sealed class OpenCoverCoverage : IBeforeTestRun, IAfterTestRun
    {
        public const string OutputDirectoryName = "coverage.opencover";

        private string outputXmlFile;
        private string assemblyDirectory;

        public void BeforeTestRun(ITestRunContext testRunContext, ICodeCoverageContext codeCoverageContext)
        {
            outputXmlFile = Path.Combine(codeCoverageContext.OutputDirectory, "coverage.opencover.xml");
            assemblyDirectory = Assembly.GetExecutingAssembly().AssemblyBaseDirectory();
            StringBuilder argments = new StringBuilder();
            argments.AppendFormat("\"-target:{0}\"", testRunContext.Exe);
            argments.AppendFormat(" \"-targetargs:{0}\"", testRunContext.Arguments.Replace("\"", "\"\""));
            argments.AppendFormat(" \"-targetdir:{0}\"", Path.GetDirectoryName(testRunContext.AssemblyToTest));
            argments.Append(" -register:user");
            argments.Append(" -mergebyhash");
            argments.AppendFormat(" \"-output:{0}\"", outputXmlFile);
            argments.AppendFormat(" \"-filter:{0}\"", codeCoverageContext.CodeCoverageFilter);
            testRunContext.Exe = Path.Combine(assemblyDirectory, "OpenCover", "OpenCover.Console.exe");
            testRunContext.Arguments = argments.ToString();
        }

        public void AfterTestRun(ICodeCoverageContext codeCoverageContext)
        {
            StringBuilder command = new StringBuilder();
            command.AppendFormat("\"{0}\"", Path.Combine(assemblyDirectory, "ReportGenerator", "ReportGenerator.exe"));
            command.AppendFormat(" \"-reports:{0}\"", outputXmlFile);
            command.AppendFormat(" \"-targetdir:{0}\"", Path.Combine(codeCoverageContext.OutputDirectory, "coverage.opencover"));
            ProcessRunner.ExecuteProcess(new ProcessStartInfo(command.ToString()));
        }
    }
}
