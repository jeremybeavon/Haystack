﻿using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.Autofac;

namespace Haystack.Examples.Interception.Autofac.Simple.Setup
{
    public sealed class InitializeTestSuiteSetupWithContainerBuilder : IInitializeTestSuite
    {
        public void InitializeTestSuite(string testSuiteName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleContainerBuilder);
        }
    }
}
