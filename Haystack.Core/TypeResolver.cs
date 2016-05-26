using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Haystack.Core
{
    public static class TypeResolver
    {
        public static IEnumerable<T> CreateInstances<T>(IEnumerable<ITypeConfiguration> types)
            where T : class
        {
            List<T> instances = new List<T>();
            List<string> invalidTypes = new List<string>();
            foreach (ITypeConfiguration type in types)
            {
                Assembly assembly = Assembly.LoadFrom(type.AssemblyFile);
                T instance = Activator.CreateInstance(assembly.GetType(type.Type, true)) as T;
                if (instance == null)
                {
                    invalidTypes.Add(type.Type);
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

        public static T CreateInstance<T>(ITypeConfiguration type)
            where T : class
        {
            return type == null ? null : CreateInstances<T>(new ITypeConfiguration[] { type }).First();
        }
    }
}
