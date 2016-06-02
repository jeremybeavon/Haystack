using Haystack.Diagnostics.Amendments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Haystack.Bootstrap
{
    public static class MethodAmendments<TInstance>
    {
        public static void BeforeMethod(TInstance instance, string methodName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            MethodInfo method = new StackFrame(1).GetMethod() as MethodInfo;
            if (method == null)
            {
                throw new NotSupportedException("BeforeMethod must be called from a method, not a constructor.");
            }

            BeforeMethodInternal(instance, method, parameters);
        }

        public static void AfterVoidMethod(TInstance instance, string methodName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            MethodInfo method = new StackFrame(1).GetMethod() as MethodInfo;
            if (method == null)
            {
                throw new NotSupportedException("AfterVoidMethod must be called from a method, not a constructor.");
            }

            AfterVoidMethodInternal(instance, method, parameters);
        }

        public static TReturnValue AfterMethod<TReturnValue>(
            TInstance instance,
            string methodName,
            object[] parameters,
            TReturnValue returnValue)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            MethodInfo method = new StackFrame(1).GetMethod() as MethodInfo;
            if (method == null)
            {
                throw new NotSupportedException("AfterMethod must be called from a method, not a constructor.");
            }

            return AfterMethodInternal(instance, method, parameters, returnValue);
        }

        public static void CatchVoidMethod<TException>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            MethodInfo method = new StackFrame(1).GetMethod() as MethodInfo;
            if (method == null)
            {
                throw new NotSupportedException("CatchVoidMethod must be called from a method, not a constructor.");
            }

            CatchVoidMethodInternal(instance, method, exception, parameters);
        }

        public static TReturnValue CatchMethod<TException, TReturnValue>(
            TInstance instance,
            string methodName,
            TException exception,
            object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            MethodInfo method = new StackFrame(1).GetMethod() as MethodInfo;
            if (method == null)
            {
                throw new NotSupportedException("AfterVoidMethod must be called from a method, not a constructor.");
            }

            return CatchMethodInternal<TException, TReturnValue>(instance, method, exception, parameters);
        }

        public static void Finally(TInstance instance, string methodName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            MethodInfo method = new StackFrame(1).GetMethod() as MethodInfo;
            if (method == null)
            {
                throw new NotSupportedException("Finally must be called from a method, not a constructor.");
            }

            FinallyInternal(instance, method, parameters);
        }

        private static void BeforeMethodInternal(TInstance instance, MethodInfo method, object[] parameters)
        {
            foreach (IBeforeMethodAmender amender in GetAmenders(AmendmentRepository.BeforeMethodAmenders, method))
            {
                amender.BeforeMethod(instance, method, parameters);
            }
        }

        private static void AfterVoidMethodInternal(TInstance instance, MethodInfo method, object[] parameters)
        {
            foreach (IAfterVoidMethodAmender amender in GetAmenders(AmendmentRepository.AfterVoidMethodAmenders, method))
            {
                amender.AfterMethod(instance, method, parameters);
            }
        }

        private static TReturnValue AfterMethodInternal<TReturnValue>(
            TInstance instance,
            MethodInfo method,
            object[] parameters,
            TReturnValue returnValue)
        {
            return GetAmenders(AmendmentRepository.AfterMethodAmenders, method)
                .Aggregate(returnValue, (value, amender) => amender.AfterMethod(instance, method, parameters, value));
        }

        private static void CatchVoidMethodInternal<TException>(
            TInstance instance,
            MethodInfo method,
            TException exception,
            object[] parameters)
        {
            foreach (ICatchVoidMethodAmender amender in GetAmenders(AmendmentRepository.CatchVoidMethodAmenders, method))
            {
                amender.CatchMethod(instance, method, exception, parameters);
            }
        }

        private static TReturnValue CatchMethodInternal<TException, TReturnValue>(
            TInstance instance,
            MethodInfo method,
            TException exception,
            object[] parameters)
        {
            return GetAmenders(AmendmentRepository.CatchMethodAmenders, method).Aggregate(
                default(TReturnValue),
                (value, amender) => amender.CatchMethod<TInstance, TException, TReturnValue>(instance, method, exception, parameters));
        }

        private static void FinallyInternal(TInstance instance, MethodInfo method, object[] parameters)
        {
            foreach (IFinallyMethodAmender amender in GetAmenders(AmendmentRepository.FinallyMethodAmenders, method))
            {
                amender.Finally(instance, method, parameters);
            }
        }

        private static IEnumerable<TAmender> GetAmenders<TAmender>(
            IEnumerable<TAmender> amenders,
            MethodInfo method)
            where TAmender : IMethodAmender
        {
            return amenders == null ?
                new TAmender[0] : 
                amenders.Where(amender => amender.AmendMethod(method));
        }
    }
}
