using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics
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
