﻿using Haystack.Core;
using Haystack.Runner.TestRunner;
using System;
using System.IO;

namespace Haystack.Runner.NUnit.TestRunner
{
    public abstract class AbstractNUnitTestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation> :
        AbstractTestIntegrationRunner<TTestIntegrationInterface, TTestIntegrationImplementation>
        where TTestIntegrationImplementation : TTestIntegrationInterface
    {
        protected sealed override string HaystackDiagnosticsDirectory
        {
            get
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\..\Haystack\Runner", FrameworkVersion.Current, "Diagnostics"));
            }
        }

        protected sealed override string RunnerFramework
        {
            get { return "NUnit"; }
        }
    }
}
