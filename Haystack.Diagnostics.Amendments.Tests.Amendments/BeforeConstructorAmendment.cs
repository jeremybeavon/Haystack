using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Tests.Amendments
{
    public sealed class BeforeConstructorAmendment : IBeforeConstructorAmender
    {
        public bool AmendConstructor(ConstructorInfo constructor)
        {
            return true;
        }
        
        public void BeforeConstructor<TInstance>(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            const string format = "BeforeConstructor(instance = {0}, parameters = {1})";
            TestTrace.TraceText = string.Format(format, typeof(TInstance).FullName, string.Join(",", parameters));
        }
    }
}
