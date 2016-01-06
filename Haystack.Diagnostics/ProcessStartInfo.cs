namespace Haystack.Diagnostics
{
    public sealed class ProcessStartInfo
    {
        public ProcessStartInfo(string command)
        {
            Command = command;
        }

        public string Command { get; private set; }

        public string WorkingDirectory { get; set; }
    }
}
