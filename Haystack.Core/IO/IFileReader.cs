namespace Haystack.Core.IO
{
    public interface IFileReader
    {
        string ReadAllText(string path);

        bool Exists(string path);
    }
}
