using Haystack.Core;
using Haystack.Diagnostics.Properties;
using System;
using System.Collections.Generic;
using System.IO;

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

        public static void ExecuteProcessInNewAppDomain(string exe, string[] args)
        {
            AppDomainSetup setup = new AppDomainSetup()
            {
                ApplicationBase = Path.GetDirectoryName(exe)
            };
            using (DisposableAppDomain appDomain = new DisposableAppDomain(Path.GetFileNameWithoutExtension(exe), setup))
            {
                CrossDomainConsoleProvider.InitializeConsole(appDomain.AppDomain);
                appDomain.AppDomain.ExecuteAssembly(exe, args);
            }
        }
    }
}
