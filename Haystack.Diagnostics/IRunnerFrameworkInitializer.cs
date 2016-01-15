using Haystack.Diagnostics.Configuration;

namespace Haystack.Diagnostics
{
    public interface IRunnerFrameworkInitializer
    {
        void InitializeRunnerFramework(IHaystackConfiguration configuration);
    }
}
