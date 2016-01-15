namespace Haystack.Amendments.Tests.TestTarget
{
    public sealed class SimpleVoidMethod
    {
        public int CallCount { get; set; }

        public void TestMethod()
        {
            CallCount++;
        }
    }
}
