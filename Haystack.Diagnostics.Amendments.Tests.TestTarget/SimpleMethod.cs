namespace Haystack.Amendments.Tests.TestTarget
{
    public sealed class SimpleMethod
    {
        public string Text { get; set; }

        public int TestMethod()
        {
            Text = "TestMethodCalled";
            return 1;
        }
    }
}
