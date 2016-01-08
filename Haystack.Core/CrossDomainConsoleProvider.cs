using AppDomainCallbackExtensions;
using System;
using System.IO;
using System.Text;
using TextSerialization;

namespace Haystack.Core
{
    public sealed class CrossDomainConsoleProvider : TextWriter
    {
        private const string providerKey = "CrossDomainConsoleProvider";
        private readonly Encoding encoding;
        private readonly AppDomain appDomain;

        private CrossDomainConsoleProvider(AppDomain appDomain)
        {
            encoding = Console.Out.Encoding;
            this.appDomain = appDomain;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }

        public static void InitializeConsole(AppDomain appDomain)
        {
            appDomain.SetData(providerKey, AppDomain.CurrentDomain);
            appDomain.DoCallBack(InitializeConsole);
        }

        public static void InitializeConsole()
        {
            Console.SetOut(new CrossDomainConsoleProvider((AppDomain)AppDomain.CurrentDomain.GetData(providerKey)));
        }

        public static void ConsoleWrite(string value)
        {
            Console.Write(value);
        }

        public override void Write(string value)
        {
            appDomain.DoCallBack(value, ConsoleWrite, new DataContractSerialization());
        }
    }
}
