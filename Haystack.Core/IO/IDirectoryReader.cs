using System.Collections.Generic;

namespace Haystack.Core.IO
{
    public interface IDirectoryReader
    {
        IEnumerable<string> GetFiles(string directory, string searchPattern);
    }
}
