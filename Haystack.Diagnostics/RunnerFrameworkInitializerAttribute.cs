using System;

namespace Haystack.Diagnostics
{
    public sealed class RunnerFrameworkInitializerAttribute : Attribute
    {
        public RunnerFrameworkInitializerAttribute(Type runnerFrameworkInitializerType)
        {
            RunnerFrameworkInitializerType = runnerFrameworkInitializerType;
        }

        public Type RunnerFrameworkInitializerType { get; private set; }
    }
}
