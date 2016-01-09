using System;
using System.Collections.Generic;
using System.Linq;

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
                    "The following types could not be converted to {0}: {1}",
                    typeof(T).FullName,
                    string.Join(",", invalidTypes));
                throw new InvalidOperationException(errorMessage);
            }

            return instances;
        }

        public static T CreateInstance<T>(string type)
            where T : class
        {
            return string.IsNullOrWhiteSpace(type) ? null : CreateInstances<T>(new string[] { type }).First();
        }
    }
}
