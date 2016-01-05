using System;

namespace Haystack.Diagnostics
{
    public sealed class DisposableAppDomain : IDisposable
    {
        public DisposableAppDomain(string friendlyName, AppDomainSetup appDomainSetup)
        {
            AppDomain = AppDomain.CreateDomain(friendlyName, null, appDomainSetup);
        }

        public AppDomain AppDomain { get; private set; }

        public void Dispose()
        {
            AppDomain.Unload(AppDomain);
        }
    }
}
