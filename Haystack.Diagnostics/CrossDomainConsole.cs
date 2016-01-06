using System;

namespace Haystack.Diagnostics
{
    public sealed class CrossDomainConsole : CrossDomainObject
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
