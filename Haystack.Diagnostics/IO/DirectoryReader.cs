using System;
using System.Collections.Generic;
using System.IO;

namespace Haystack.Diagnostics.IO
{
    public sealed class DirectoryReader : IDirectoryReader
    {
        public IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            return Directory.GetFiles(directory, searchPattern);
        }
    }
}
