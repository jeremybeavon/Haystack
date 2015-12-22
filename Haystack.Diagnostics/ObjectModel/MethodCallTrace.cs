using System.Collections.Generic;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class MethodCallTrace
    {
        public MethodCallTrace()
        {
            MethodCallThreads = new List<MethodCallThreadTrace>();
            Objects = new List<ObjectInstance>();
            Types = new List<ObjectType>();
        }

        public string Description { get; set; }

        public List<MethodCallThreadTrace> MethodCallThreads { get; set; }

        public List<ObjectInstance> Objects { get; set; }

        public List<ObjectType> Types { get; set; }
    }
}
