using Haystack.Diagnostics.Properties;
using System.Collections.Generic;

namespace Haystack.Diagnostics
{
    public static class ProcessRunner
    {
        public static void ExecuteProcess(ProcessStartInfo processStartInfo)
        {
            IDictionary<string, string> properties = new Dictionary<string, string>()
            {
                { "Command", processStartInfo.Command },
                { "WorkingDirectory", processStartInfo.WorkingDirectory ?? string.Empty }
            };
            MsBuildRunner.RunMsBuildXml(Resources.ExecuteProcess, properties);
        }
    }
}
