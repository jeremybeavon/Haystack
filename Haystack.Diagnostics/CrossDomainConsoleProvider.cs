using System;

namespace Haystack.Diagnostics
{
    public sealed class CrossDomainConsoleProvider : CrossDomainObject
    {
        public void InitializeConsole(CrossDomainConsole console)
        {
            Console.SetOut(new CrossDomainTextWriter(console));
        }
    }
}
