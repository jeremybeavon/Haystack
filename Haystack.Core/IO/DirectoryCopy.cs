using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace Haystack.Core.IO
{
    public static class DirectoryCopy
    {
        public static void CopyDirectory(string source, string destination)
        {
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }

            FileSystem.CopyDirectory(source, destination);
        }
    }
}
