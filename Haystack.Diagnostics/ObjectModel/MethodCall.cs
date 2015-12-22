using System;
using System.Collections.Generic;
using MsgPack.Serialization;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class MethodCall
    {
        public MethodCall()
        {
            Parameters = new List<MethodParameter>();
            MethodCalls = new List<MethodCall>();
        }

        [MessagePackMember(0)]
        public int DeclaringTypeIndex { get; set; }

        public ObjectType DeclaringType { get; set; }

        [MessagePackMember(1)]
        public int InstanceIndex { get; set; }

        public ObjectInstance Instance { get; set; }

        [MessagePackMember(2)]
        public string MethodName { get; set; }

        [MessagePackMember(3)]
        public List<MethodParameter> Parameters { get; set; }

        [MessagePackMember(4)]
        public PropertyType PropertyType { get; set; }

        [MessagePackMember(5)]
        public int ReturnTypeIndex { get; set; }

        public ObjectType ReturnType { get; set; }

        [MessagePackMember(6)]
        public Value ReturnValue { get; set; }

        [MessagePackMember(7)]
        public List<MethodCall> MethodCalls { get; set; }
    }
}
