﻿namespace Haystack.Diagnostics.Amendments.Tests.TestTarget
{
    public sealed class SimpleProperty
    {
        private string testProperty;

        public SimpleProperty(string testProperty)
        {
            this.testProperty = testProperty;
        }

        public string TestProperty
        {
            get { return testProperty; }
            set { testProperty = value; }
        }
    }
}
