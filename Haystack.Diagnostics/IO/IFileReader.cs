namespace Haystack.Diagnostics.IO
{
    public interface IFileReader
    {
        string ReadAllText(string path);

        bool Exists(string path);
    }
}
