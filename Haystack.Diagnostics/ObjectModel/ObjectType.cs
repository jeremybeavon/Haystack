using System;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class ObjectType : IObjectType, IEquatable<ObjectType>
    {
        public ObjectType()
        {
        }

        public ObjectType(string assemblyName, string typeName)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
        }

        public string AssemblyName { get; set; }

        public string TypeName { get; set; }

        public override string ToString()
        {
            return string.Format("AssemblyName: {0}, TypeName: {1}", AssemblyName, TypeName);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectType);
        }

        public bool Equals(ObjectType other)
        {
            return other != null && AssemblyName == other.AssemblyName && TypeName == other.TypeName;
        }
    }
}
