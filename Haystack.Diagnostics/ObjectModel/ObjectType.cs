namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class ObjectType
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
    }
}
