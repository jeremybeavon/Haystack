using System;
using System.IO;

namespace Haystack.Diagnostics.IO
{
    public sealed class FileReader : IFileReader
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
