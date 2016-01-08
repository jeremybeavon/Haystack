using System;
using System.Collections.Generic;

namespace Haystack.Core
{
    public static class TypeResolver
    {
        public static IEnumerable<T> CreateInstances<T>(IEnumerable<string> types)
            where T : class
        {
            List<T> instances = new List<T>();
            List<string> invalidTypes = new List<string>();
            foreach (string type in types)
            {
                T instance = Activator.CreateInstance(Type.GetType(type, true)) as T;
                if (instance == null)
                {
                    invalidTypes.Add(type);
                }
                else
                {
                    instances.Add(instance);
                }
            }

            if (invalidTypes.Count != 0)
            {
                string errorMessage = string.Format(
                    "The following type could not be converted to {0}: {1}",
                    typeof(T).FullName,
                    string.Join(",", invalidTypes));
                throw new InvalidOperationException(errorMessage);
            }

            return instances;
        }
    }
}
