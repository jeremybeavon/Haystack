namespace Haystack.Amendments.Tests
{
    public sealed class StrongNamedAmendmentTestRunner : AmendmentTestRunner
    {
        public StrongNamedAmendmentTestRunner(string strongNameKey)
        {
            StrongNameKey = strongNameKey;
        }
    }
}
