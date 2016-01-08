﻿using FluentAssertions;
using Haystack.Examples.Amendments.SimpleProperty;
using NUnit.Framework;

namespace Haystack.Examples.SimpleProperty.Tests
{
    [TestFixture]
    public sealed class SimplePropertyTests
    {
        [Test]
        public void TestSimpleProperty()
        {
            PropertyWrapper wrapper = new PropertyWrapper()
            {
                Value = "BeforeTest"
            };
            wrapper.Value.Should().Be("BeforeTest");
        }
    }
}
