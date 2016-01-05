using Haystack.Diagnostics;
using System;

namespace Haystack.Amendments.Setup
{
    public sealed class AmendmentConsoleProvider : CrossDomainObject
    {
        public void InitializeConsole(AmendmentConsole console)
        {
            Console.SetOut(new CrossDomainTextWriter(console));
        }
    }
}
