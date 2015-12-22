using System.Collections.Generic;

namespace Haystack.Diagnostics.IO
{
    public interface IDirectoryReader
    {
        IEnumerable<string> GetFiles(string directory, string searchPattern);
    }
}
