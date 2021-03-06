﻿using MsgPack.Serialization;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class Value : IValue
    {
        [MessagePackMember(0)]
        public string RawValue { get; set; }

        [MessagePackMember(1)]
        public int ObjectIndex { get; set; }

        public ObjectInstance Object { get; set; }

        IObjectInstance IValue.Object
        {
            get { return Object; }
        }
    }
}
