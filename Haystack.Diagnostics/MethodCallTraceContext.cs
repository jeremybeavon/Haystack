namespace Haystack.Diagnostics
{
    public static class MethodCallTraceContext
    {
        private static MethodCallTraceProvider methodCallTrace;
        private static readonly object methodCallTraceLock = new object();

        public static MethodCallTraceProvider MethodCallTrace
        {
            get
            {
                if (methodCallTrace != null)
                    return methodCallTrace;

                lock (methodCallTraceLock)
                {
                    methodCallTrace = new MethodCallTraceProvider();
                    return methodCallTrace;
                }
            }
        }

        public static void Save(string fileName, string description)
        {
            MethodCallTrace.Save(fileName, description);
            methodCallTrace = null;
        }
    }
}
