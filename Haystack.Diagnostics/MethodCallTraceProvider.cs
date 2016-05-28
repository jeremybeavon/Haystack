using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Haystack.Diagnostics.ObjectModel;
using MsgPack.Serialization;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Diagnostics
{
    public class MethodCallTraceProvider
    {
        private const int NullValue = -1;
        private readonly ConcurrentDictionary<int, Stack<MethodCall>> methodCallStack;
        private readonly ConcurrentDictionary<object, IndexedObject<ObjectInstance>> objects;
        private readonly ConcurrentDictionary<Type, IndexedObject<ObjectType>> types;
        private int currentObjectId;
        private int currentTypeId;

        public MethodCallTraceProvider()
        {
            methodCallStack = new ConcurrentDictionary<int, Stack<MethodCall>>();
            objects = new ConcurrentDictionary<object, IndexedObject<ObjectInstance>>();
            types = new ConcurrentDictionary<Type, IndexedObject<ObjectType>>();
            currentObjectId = -1;
            currentTypeId = -1;
        }

        public void EnterMethodCall(MethodCall methodCall)
        {
            AddMethodCallToStack(methodCall);
        }

        public MethodCall ExitMethodCall()
        {
            return RemoveMethodCallFromStack();
        }
        
        public void EnterPropertyGet<TInstance>(TInstance instance, string propertyName)
        {
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = GetTypeIndex(typeof(TInstance)),
                InstanceIndex = GetObjectIndex(instance),
                MethodName = propertyName,
                PropertyType = PropertyType.Get
            };
            AddMethodCallToStack(methodCall);
        }

        public void ExitPropertyGet<TProperty>(TProperty value)
        {
            MethodCall methodCall = RemoveMethodCallFromStack();
            methodCall.ReturnTypeIndex = GetTypeIndex(typeof(TProperty));
            methodCall.ReturnValue = GetValue(value);
        }

        public void EnterPropertySet<TInstance, TProperty>(TInstance instance, string propertyName, TProperty value)
        {
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = GetTypeIndex(typeof(TInstance)),
                InstanceIndex = GetObjectIndex(instance),
                MethodName = propertyName,
                PropertyType = PropertyType.Set,
                Parameters = new List<MethodParameter>
                {
                    new MethodParameter()
                    {
                        ParameterTypeIndex = GetTypeIndex(typeof(TProperty)),
                        ParameterName = "value",
                        Value = GetValue(value)
                    }
                },
                ReturnTypeIndex = GetTypeIndex(typeof(void)),
                ReturnValue = GetValue(null)
            };
            AddMethodCallToStack(methodCall);
        }

        public void ExitPropertySet()
        {
            RemoveMethodCallFromStack();
        }

        public MethodCallTrace BuildMethodCallTrace()
        {
            return new MethodCallTrace
            {
                MethodCallThreads = methodCallStack.Select(entry => new MethodCallThreadTrace(entry.Key, entry.Value.Peek().MethodCalls)).ToList(),
                Objects = objects.Values.OrderBy(entry => entry.Index).Select(entry => entry.Object).ToList(),
                Types = types.Values.OrderBy(entry => entry.Index).Select(entry => entry.Object).ToList()
            };
        }

        public void Save(string fileName, string description)
        {
            MethodCallTrace methodCallTrace = BuildMethodCallTrace();
            methodCallTrace.Description = description;
            Save(fileName, methodCallTrace);
        }

        public static void Save(string fileName, MethodCallTrace methodCallTrace)
        {
            using (Stream stream = File.Create(fileName))
            {
                SerializationContext.Default.GetSerializer<MethodCallTrace>().Pack(stream, methodCallTrace);
            }
        }

        public static MethodCallTrace Load(string fileName)
        {
            MethodCallTrace methodCallTrace;
            using (Stream stream = File.OpenRead(fileName))
            {
                methodCallTrace = SerializationContext.Default.GetSerializer<MethodCallTrace>().Unpack(stream);
            }

            Initialize(methodCallTrace);
            return methodCallTrace;
        }

        public static void Initialize(MethodCallTrace methodCallTrace)
        {
            foreach (ObjectInstance @object in methodCallTrace.Objects)
                @object.Type = methodCallTrace.Types[@object.TypeIndex];

            foreach (MethodCall methodCall in methodCallTrace.MethodCallThreads.SelectMany(thread => thread.MethodCalls))
                Initialize(methodCallTrace, methodCall);
        }

        public Value GetValue(object value)
        {
            bool isSimpleValue = IsSimpleValue(value);
            return new Value
            {
                RawValue = isSimpleValue && value != null ? value.ToString() : null,
                ObjectIndex = isSimpleValue ? NullValue : GetObjectIndex(value)
            };
        }

        public int GetObjectIndex(object instance)
        {
            return instance == null ? NullValue : objects.GetOrAdd(instance, CreateObjectIndex).Index;
        }

        public int GetTypeIndex(Type type)
        {
            return types.GetOrAdd(type, CreateTypeIndex).Index;
        }

        public List<MethodParameter> GetParameters(IEnumerable<object> values, ParameterInfo[] parameters)
        {
            return values.Select((value, index) =>
            {
                ParameterInfo parameter = parameters[index];
                return new MethodParameter
                {
                    ParameterTypeIndex = GetTypeIndex(parameter.ParameterType),
                    ParameterName = parameter.ParameterType.Name,
                    Modifier = GetParameterModifier(parameter)
                };
            }).ToList();
        }

        private static void Initialize(MethodCallTrace methodCallTrace, MethodCall methodCall)
        {
            methodCall.DeclaringType = methodCallTrace.Types[methodCall.DeclaringTypeIndex];
            methodCall.Instance = methodCallTrace.Objects[methodCall.InstanceIndex];
            methodCall.ReturnType = methodCallTrace.Types[methodCall.ReturnTypeIndex];
            Initialize(methodCallTrace, methodCall.ReturnValue);
            foreach (MethodParameter parameter in methodCall.Parameters)
            {
                parameter.ParameterType = methodCallTrace.Types[parameter.ParameterTypeIndex];
                Initialize(methodCallTrace, parameter.Value);
                Initialize(methodCallTrace, parameter.OutputValue);
            }

            foreach (MethodCall childMethodCall in methodCall.MethodCalls)
            {
                childMethodCall.CalledBy = methodCall;
                Initialize(methodCallTrace, childMethodCall);
            }
        }
        
        private static void Initialize(MethodCallTrace methodCallTrace, Value value)
        {
            if (value != null && value.ObjectIndex != NullValue)
                value.Object = methodCallTrace.Objects[value.ObjectIndex];
        }

        private void AddMethodCallToStack(MethodCall methodCall)
        {
            Stack<MethodCall> callStack = methodCallStack.GetOrAdd(Thread.CurrentThread.ManagedThreadId, CreateMethodCallStack);
            methodCall.Index = callStack.Peek().MethodCalls.Count;
            callStack.Peek().MethodCalls.Add(methodCall);
            callStack.Push(methodCall);
        }

        private Stack<MethodCall> CreateMethodCallStack(int threadId)
        {
            return new Stack<MethodCall>(new[] { new MethodCall() });
        }

        private MethodCall RemoveMethodCallFromStack()
        {
            return methodCallStack[Thread.CurrentThread.ManagedThreadId].Pop();
        }

        private IndexedObject<ObjectInstance> CreateObjectIndex(object instance)
        {
            return new IndexedObject<ObjectInstance>
            {
                Object = new ObjectInstance
                {
                    TypeIndex = GetTypeIndex(instance.GetType())
                },
                Index = Interlocked.Increment(ref currentObjectId)
            };
        }

        private IndexedObject<ObjectType> CreateTypeIndex(Type type)
        {
            return new IndexedObject<ObjectType>
            {
                Object = new ObjectType
                {
                    AssemblyName = type.Assembly.GetName().Name,
                    TypeName = type.FullName
                },
                Index = Interlocked.Increment(ref currentTypeId)
            };
        }

        private static bool IsSimpleValue(object value)
        {
            return value == null || Type.GetTypeCode(value.GetType()) != TypeCode.Object;
        }

        private static ParameterModifier GetParameterModifier(ParameterInfo parameter)
        {
            if (parameter.IsOut)
                return ParameterModifier.Out;

            if (parameter.ParameterType.IsByRef)
                return ParameterModifier.Ref;

            return ParameterModifier.None;
        }
    }
}
