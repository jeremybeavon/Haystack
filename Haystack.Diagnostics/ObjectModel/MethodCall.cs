﻿using MsgPack.Serialization;
using System.Collections.Generic;
using System.Threading;
using System;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class MethodCall : IMethodCall
    {
        private static int nextMethodCallId;

        public MethodCall()
        {
            Parameters = new List<MethodParameter>();
            MethodCalls = new List<MethodCall>();
            ResetMethodCallId();
        }

        [MessagePackMember(0)]
        public int MethodCallId { get; set; }

        [MessagePackMember(1)]
        public int Index { get; set; }

        [MessagePackMember(2)]
        public int DeclaringTypeIndex { get; set; }

        public ObjectType DeclaringType { get; set; }

        [MessagePackMember(3)]
        public int InstanceIndex { get; set; }

        public ObjectInstance Instance { get; set; }

        [MessagePackMember(4)]
        public string MethodName { get; set; }

        [MessagePackMember(5)]
        public List<MethodParameter> Parameters { get; set; }

        [MessagePackMember(6)]
        public PropertyType PropertyType { get; set; }

        [MessagePackMember(7)]
        public int ReturnTypeIndex { get; set; }

        public ObjectType ReturnType { get; set; }

        [MessagePackMember(8)]
        public Value ReturnValue { get; set; }

        [MessagePackMember(9)]
        public List<MethodCall> MethodCalls { get; set; }

        public MethodCall CalledBy { get; set; }

        public IMethodCallThreadTrace Thread { get; set; }

        public IMethodCallTrace Trace { get; set; }

        IObjectType IMethodCall.DeclaringType
        {
            get { return DeclaringType; }
        }

        IObjectInstance IMethodCall.Instance
        {
            get { return Instance; }
        }

        IEnumerable<IMethodParameter> IMethodCall.Parameters
        {
            get { return Parameters; }
        }

        IObjectType IMethodCall.ReturnType
        {
            get { return ReturnType; }
        }

        IValue IMethodCall.ReturnValue
        {
            get { return ReturnValue; }
        }

        IEnumerable<IMethodCall> IMethodCall.MethodCalls
        {
            get { return MethodCalls; }
        }

        IMethodCall IMethodCall.CalledBy
        {
            get { return CalledBy; }
        }

        public MethodCall ToMethodCall()
        {
            return this;
        }

        public void ResetMethodCallId()
        {
            MethodCallId = Interlocked.Increment(ref nextMethodCallId);
        }
    }
}
