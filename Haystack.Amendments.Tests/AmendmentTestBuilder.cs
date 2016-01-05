using Haystack.Diagnostics.Configuration;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace Haystack.Amendments.Tests
{
    public static class AmendmentTestBuilder
    {
        public static void SetupAmendmentAssembly(string testName, string targetDll, HaystackConfiguration configuration, string strongNameKey = null)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string testDirectory = Path.Combine(baseDirectory, testName);
            Directory.Delete(testDirectory, true);
            FileSystem.CopyDirectory(Path.GetDirectoryName(targetDll), testDirectory);
            string assemblyName = Path.GetFileNameWithoutExtension(targetDll);
            (new AmendmentSetupProvider(Path.Combine(testDirectory, Path.GetFileName(targetDll)), configuration, strongNameKey)
            {
                AfterthoughtAmenderExe = Path.Combine(baseDirectory, AmendmentSetupProvider.AfterthoughtAmenderExeFileName),
                AmendmentsDll = Path.Combine(baseDirectory, AmendmentSetupProvider.AmendmentsDllFileName)
            }).SetupIfNecessary();
        }
    }
}
