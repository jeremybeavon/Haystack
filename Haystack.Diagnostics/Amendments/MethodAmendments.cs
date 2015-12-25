﻿using System.Collections.Generic;
using System.Linq;

namespace Haystack.Diagnostics.Amendments
{
    public static class MethodAmendments<TInstance>
    {
        public static void BeforeMethod(TInstance instance, string methodName, object[] parameters)
        {
            IEnumerable<IBeforeMethodAmender> amenders = AmendmentRepository.BeforeMethodAmenders;
            foreach (IBeforeMethodAmender amender in GetAmenders(amenders, methodName, parameters))
            {
                amender.BeforeMethod(instance, methodName, parameters);
            }
        }

        public static void AfterVoidMethod(TInstance instance, string methodName, object[] parameters)
        {
            IEnumerable<IAfterVoidMethodAmender> amenders = AmendmentRepository.AfterVoidMethodAmenders;
            foreach (IAfterVoidMethodAmender amender in GetAmenders(amenders, methodName, parameters))
            {
                amender.AfterMethod(instance, methodName, parameters);
            }
        }

        public static TReturnValue AfterMethod<TReturnValue>(
            TInstance instance,
            string methodName,
            object[] parameters,
            TReturnValue returnValue)
        {
            return GetAmenders(AmendmentRepository.AfterMethodAmenders, methodName, parameters)
                .Aggregate(returnValue, (value, amender) => amender.AfterMethod(instance, methodName, parameters, value));
        }

        public static void CatchVoidMethod<TException>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            IEnumerable<ICatchVoidMethodAmender> amenders = AmendmentRepository.CatchVoidMethodAmenders;
            foreach (ICatchVoidMethodAmender amender in GetAmenders(amenders, methodName, parameters))
            {
                amender.CatchMethod(instance, methodName, exception, parameters);
            }
        }

        public static TReturnValue CatchMethod<TException, TReturnValue>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            return GetAmenders(AmendmentRepository.CatchMethodAmenders, methodName, parameters).Aggregate(
                default(TReturnValue), 
                (value, amender) => amender.CatchMethod<TInstance, TException, TReturnValue>(instance, methodName, exception, parameters));
        }

        public static void Finally(TInstance instance, string methodName, object[] parameters)
        {
            IEnumerable<IFinallyMethodAmender> amenders = AmendmentRepository.FinallyMethodAmenders;
            foreach (IFinallyMethodAmender amender in GetAmenders(amenders, methodName, parameters))
            {
                amender.Finally(instance, methodName, parameters);
            }
        }

        private static IEnumerable<TAmender> GetAmenders<TAmender>(
            IEnumerable<TAmender> amenders,
            string methodName,
            object[] parameters)
            where TAmender : IMethodAmender
        {
            return amenders.Where(amender => amender.AmendMethod(typeof(TInstance), methodName, parameters));
        }
    }
}
