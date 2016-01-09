using Haystack.Diagnostics.Amendments;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Bootstrap
{
    public static class MethodAmendments<TInstance>
    {
        public static void BeforeMethod(TInstance instance, string methodName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            BeforeMethodInternal(instance, methodName, parameters);
        }

        public static void AfterVoidMethod(TInstance instance, string methodName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            AfterVoidMethodInternal(instance, methodName, parameters);
        }

        public static TReturnValue AfterMethod<TReturnValue>(
            TInstance instance,
            string methodName,
            object[] parameters,
            TReturnValue returnValue)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            return AfterMethodInternal(instance, methodName, parameters, returnValue);
        }

        public static void CatchVoidMethod<TException>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            CatchVoidMethodInternal(instance, methodName, exception, parameters);
        }

        public static TReturnValue CatchMethod<TException, TReturnValue>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            return CatchMethodInternal<TException, TReturnValue>(instance, methodName, exception, parameters);
        }

        public static void Finally(TInstance instance, string methodName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            FinallyInternal(instance, methodName, parameters);
        }

        private static void BeforeMethodInternal(TInstance instance, string methodName, object[] parameters)
        {
            IEnumerable<IBeforeMethodAmender> amenders = AmendmentRepository.BeforeMethodAmenders;
            if (amenders != null)
            {
                foreach (IBeforeMethodAmender amender in GetAmenders(amenders, methodName, parameters))
                {
                    amender.BeforeMethod(instance, methodName, parameters);
                }
            }
        }

        private static void AfterVoidMethodInternal(TInstance instance, string methodName, object[] parameters)
        {
            IEnumerable<IAfterVoidMethodAmender> amenders = AmendmentRepository.AfterVoidMethodAmenders;
            if (amenders != null)
            {
                foreach (IAfterVoidMethodAmender amender in GetAmenders(amenders, methodName, parameters))
                {
                    amender.AfterMethod(instance, methodName, parameters);
                }
            }
        }

        private static TReturnValue AfterMethodInternal<TReturnValue>(
            TInstance instance,
            string methodName,
            object[] parameters,
            TReturnValue returnValue)
        {
            return GetAmenders(AmendmentRepository.AfterMethodAmenders, methodName, parameters)
                .Aggregate(returnValue, (value, amender) => amender.AfterMethod(instance, methodName, parameters, value));
        }

        private static void CatchVoidMethodInternal<TException>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            IEnumerable<ICatchVoidMethodAmender> amenders = AmendmentRepository.CatchVoidMethodAmenders;
            if (amenders != null)
            {
                foreach (ICatchVoidMethodAmender amender in GetAmenders(amenders, methodName, parameters))
                {
                    amender.CatchMethod(instance, methodName, exception, parameters);
                }
            }
        }

        private static TReturnValue CatchMethodInternal<TException, TReturnValue>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            return GetAmenders(AmendmentRepository.CatchMethodAmenders, methodName, parameters).Aggregate(
                default(TReturnValue),
                (value, amender) => amender.CatchMethod<TInstance, TException, TReturnValue>(instance, methodName, exception, parameters));
        }

        private static void FinallyInternal(TInstance instance, string methodName, object[] parameters)
        {
            IEnumerable<IFinallyMethodAmender> amenders = AmendmentRepository.FinallyMethodAmenders;
            if (amenders != null)
            {
                foreach (IFinallyMethodAmender amender in GetAmenders(amenders, methodName, parameters))
                {
                    amender.Finally(instance, methodName, parameters);
                }
            }
        }

        private static IEnumerable<TAmender> GetAmenders<TAmender>(
            IEnumerable<TAmender> amenders,
            string methodName,
            object[] parameters)
            where TAmender : IMethodAmender
        {
            return amenders == null ?
                new TAmender[0] : 
                amenders.Where(amender => amender.AmendMethod(typeof(TInstance), methodName, parameters));
        }
    }
}
