﻿using Haystack.Diagnostics.TestIntegration;
using Haystack.Interception.Ninject;

namespace Haystack.Examples.Interception.Ninject.Simple.Setup
{
    public sealed class InitializeTestMethodSetup : IInitializeTestMethod
    {
        public void InitializeTestMethod(string testMethodName)
        {
            HaystackInterceptor.SetUp(DependencyManager.SimpleKernel);
        }
    }
}