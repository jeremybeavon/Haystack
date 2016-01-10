using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IInterceptionConfiguration
    {
        string InterceptionFramework { get; }

        string InterceptionFrameworkVersion { get; }
    }
}
