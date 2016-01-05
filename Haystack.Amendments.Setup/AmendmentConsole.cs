using Haystack.Diagnostics;
using System;

namespace Haystack.Amendments.Setup
{
    public sealed class AmendmentConsole : CrossDomainObject
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
