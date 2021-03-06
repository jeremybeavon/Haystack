﻿using Haystack.Diagnostics.TestIntegration;
using Haystack.Diagnostics.Interception.StructureMap;

namespace Haystack.Examples.Interception.StructureMap.Simple.Setup
{
    public sealed class InitializeTestFrameworkSetup : IInitializeTestFramework
    {
        public void InitializeTestFramework()
        {
            HaystackInterceptionPolicy.SetUp(DependencyManager.SimpleContainer);
        }
    }
}
