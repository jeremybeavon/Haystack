using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Amendments.Setup
{
    public sealed class CrossDomainTextWriter : TextWriter
    {
        private readonly Encoding encoding;
        private readonly AmendmentConsole console;

        public CrossDomainTextWriter(AmendmentConsole console)
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
