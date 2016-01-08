using System;

namespace Haystack.Core
{
    public sealed class CrossDomainConsole : CrossDomainObject
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
