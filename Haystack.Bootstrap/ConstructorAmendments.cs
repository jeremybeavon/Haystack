using Haystack.Diagnostics.Amendments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Haystack.Bootstrap
{
    public static class ConstructorAmendments<TInstance>
    {
        public static void BeforeConstructor(TInstance instance, string constructorName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            ConstructorInfo constructor = new StackFrame(1).GetMethod() as ConstructorInfo;
            if (constructor == null)
            {
                throw new NotSupportedException("BeforeConstructor must be called from a constructor, not a method.");
            }

            BeforeConstructor(instance, constructor, parameters);
        }

        public static void AfterConstructor(TInstance instance, string constructorName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            ConstructorInfo constructor = new StackFrame(1).GetMethod() as ConstructorInfo;
            if (constructor == null)
            {
                throw new NotSupportedException("AfterConstructor must be called from a constructor, not a method.");
            }

            AfterConstructor(instance, constructor, parameters);
        }

        public static void CatchConstructor(TInstance instance, string constructorName, object[] parameters)
        {
            HaystackBootstrapInitializer.InitializeIfNecessary();
            ConstructorInfo constructor = new StackFrame(1).GetMethod() as ConstructorInfo;
            if (constructor == null)
            {
                throw new NotSupportedException("BeforeConstructor must be called from a constructor, not a method.");
            }

            CatchConstructor(instance, constructor, parameters);
        }

        private static void BeforeConstructor(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            ProcessConstructor(
                AmendmentRepository.BeforeConstructorAmenders,
                constructor,
                amendment => amendment.BeforeConstructor(instance, constructor, parameters));
        }

        private static void AfterConstructor(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            ProcessConstructor(
                AmendmentRepository.AfterConstructorAmenders,
                constructor,
                amendment => amendment.AfterConstructor(instance, constructor, parameters));
        }

        private static void CatchConstructor(TInstance instance, ConstructorInfo constructor, object[] parameters)
        {
            ProcessConstructor(
                AmendmentRepository.CatchConstructorAmenders,
                constructor,
                amendment => amendment.CatchConstructor(instance, constructor, parameters));
        }

        private static void ProcessConstructor<TAmender>(
            IEnumerable<TAmender> amenders,
            ConstructorInfo constructor,
            Action<TAmender> action)
            where TAmender : IConstructorAmender
        {
            if (amenders != null)
            {
                foreach (TAmender amendment in amenders.Where(amender => amender.AmendConstructor(constructor)))
                {
                    action(amendment);
                }
            }
        }
    }
}
