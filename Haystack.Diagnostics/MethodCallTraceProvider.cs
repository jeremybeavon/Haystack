using Haystack.Diagnostics.ObjectModel;
using MsgPack.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using ParameterModifier = Haystack.Diagnostics.ObjectModel.ParameterModifier;

namespace Haystack.Diagnostics
{
    public class MethodCallTraceProvider
    {
        private const int NullValue = -1;
        private readonly ConcurrentDictionary<int, MethodCallStack> methodCallStack;
        private readonly ConcurrentDictionary<object, IndexedObject<ObjectInstance>> objects;
        private readonly ConcurrentDictionary<Type, IndexedObject<ObjectType>> types;
        private int currentObjectId;
        private int currentTypeId;

        public MethodCallTraceProvider()
        {
            methodCallStack = new ConcurrentDictionary<int, MethodCallStack>();
            objects = new ConcurrentDictionary<object, IndexedObject<ObjectInstance>>();
            types = new ConcurrentDictionary<Type, IndexedObject<ObjectType>>();
            currentObjectId = -1;
            currentTypeId = -1;
        }

        public void EnterConstructorCall(object instance, ConstructorInfo constructor, object[] parameters)
        {
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = GetTypeIndex(constructor.DeclaringType),
                InstanceIndex = GetObjectIndex(instance),
                MethodName = constructor.Name,
                Parameters = GetParameters(parameters, constructor.GetParameters()),
            };
            EnterMethodCall(methodCall);
        }

        public void ExitConstructorCall(object[] parameters)
        {
            ExitMethodCall(null, parameters);
        }

        public void EnterMethodCall(object instance, MethodInfo method, object[] parameters)
        {
            MethodCall methodCall = new MethodCall()
            {
                DeclaringTypeIndex = GetTypeIndex(method.DeclaringType),
                InstanceIndex = GetObjectIndex(instance),
                MethodName = method.Name,
                Parameters = GetParameters(parameters, method.GetParameters()),
                ReturnTypeIndex = GetTypeIndex(method.ReturnType),
            };
            if (method.Attributes.HasFlag(MethodAttributes.SpecialName) && Regex.IsMatch(method.Name, "^[gs]et_"))
            {
                methodCall.PropertyType = method.Name.StartsWith("get_") ? PropertyType.Get : PropertyType.Set;
                methodCall.MethodName = methodCall.MethodName.Substring(4);
            }

            EnterMethodCall(methodCall);
        }

        public void EnterMethodCall(MethodCall methodCall)
        {
            AddMethodCallToStack(methodCall);
        }

        public void ExitMethodCall(object returnValue, object[] parameters)
        {
            MethodCall methodCall = RemoveMethodCallFromStack();
            if (methodCall == null)
            {
                return;
            }
            
            methodCall.ReturnValue = GetValue(returnValue);
            foreach (int index in methodCall.Parameters.Where(param => param.Modifier != ParameterModifier.None).Select((value, index) => index))
            {
                methodCall.Parameters[index].OutputValue = GetValue(parameters[index]);
            }
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
            if (methodCall != null)
            {
                methodCall.ReturnTypeIndex = GetTypeIndex(typeof(TProperty));
                methodCall.ReturnValue = GetValue(value);
            }
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
                MethodCallThreads = methodCallStack.Select(entry => new MethodCallThreadTrace(entry.Key, entry.Value.CallStack.Peek().MethodCalls)).ToList(),
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
            
            Initialize(methodCallTrace);
            foreach (MethodCallThreadTrace threadTrace in methodCallTrace.MethodCallThreads)
            {
                using (TextWriter writer = File.CreateText(fileName + "." + threadTrace.ThreadId + ".txt"))
                {
                    new MethodCallThreadTraceText(threadTrace).WriteText(writer);
                }
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

            foreach (MethodCallThreadTrace thread in methodCallTrace.MethodCallThreads)
            {
                foreach (MethodCall methodCall in thread.MethodCalls)
                {
                    Initialize(methodCallTrace, thread, methodCall);
                }
            }
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
            if (instance == null)
            {
                return NullValue;
            }

            MethodCallStack callStack = methodCallStack.GetOrAdd(Thread.CurrentThread.ManagedThreadId, threadId => new MethodCallStack());
            int objectIndex = NullValue;
            if (!callStack.IsInGetHashCode)
            {
                callStack.IsInGetHashCode = true;
                try
                {
                    objectIndex = objects.GetOrAdd(instance, CreateObjectIndex).Index;
                }
                finally
                {
                    callStack.IsInGetHashCode = false;
                }
            }

            return objectIndex;
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

        private static void Initialize(MethodCallTrace methodCallTrace, MethodCallThreadTrace thread, MethodCall methodCall)
        {
            methodCall.DeclaringType = methodCallTrace.Types[methodCall.DeclaringTypeIndex];
            methodCall.Instance = methodCallTrace.Objects[methodCall.InstanceIndex];
            methodCall.ReturnType = methodCallTrace.Types[methodCall.ReturnTypeIndex];
            methodCall.Trace = methodCallTrace;
            methodCall.Thread = thread;
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
                Initialize(methodCallTrace, thread, childMethodCall);
            }
        }
        
        private static void Initialize(MethodCallTrace methodCallTrace, Value value)
        {
            if (value != null && value.ObjectIndex != NullValue)
                value.Object = methodCallTrace.Objects[value.ObjectIndex];
        }

        private void AddMethodCallToStack(MethodCall methodCall)
        {
            MethodCallStack callStack = methodCallStack.GetOrAdd(Thread.CurrentThread.ManagedThreadId, threadId => new MethodCallStack());
            if (callStack.IsInGetHashCode)
            {
                return;
            }

            methodCall.Index = callStack.CallStack.Peek().MethodCalls.Count;
            callStack.CallStack.Peek().MethodCalls.Add(methodCall);
            callStack.CallStack.Push(methodCall);
        }
        
        private MethodCall RemoveMethodCallFromStack()
        {
            MethodCallStack callStack = methodCallStack[Thread.CurrentThread.ManagedThreadId];
            return callStack.IsInGetHashCode ? null : callStack.CallStack.Pop();
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
