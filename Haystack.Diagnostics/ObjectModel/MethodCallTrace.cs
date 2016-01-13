using System.Collections.Generic;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class MethodCallTrace : IMethodCallTrace
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

        IEnumerable<IMethodCallThreadTrace> IMethodCallTrace.MethodCallThreads
        {
            get { return MethodCallThreads; }
        }

        IEnumerable<IObjectInstance> IMethodCallTrace.Objects
        {
            get { return Objects; }
        }

        IEnumerable<IObjectType> IMethodCallTrace.Types
        {
            get { return Types; }
        }
    }
}
