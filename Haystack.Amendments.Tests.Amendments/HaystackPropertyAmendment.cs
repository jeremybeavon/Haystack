﻿using Haystack.Diagnostics.Amendments;
using System;
using System.Reflection;

namespace Haystack.Amendments.Tests.Amendments
{
    public sealed class HaystackPropertyAmendment : IPropertyAmender
    {
        public bool AmendProperty(PropertyInfo property)
        {
            return true;
        }

        public bool AmendProperty(Type type, string propertyName)
        {
            return true;
        }
    }
}