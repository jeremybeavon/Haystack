using System;
using System.IO;
using System.Text;

namespace Haystack.Core
{
    public sealed class CrossDomainTextWriter : TextWriter
    {
        private readonly Encoding encoding;
        private readonly CrossDomainConsole console;

        public CrossDomainTextWriter(CrossDomainConsole console)
        {
            encoding = Console.Out.Encoding;
            this.console = console;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }

        public override void Write(string value)
        {
            console.Write(value);
        }
    }
}
