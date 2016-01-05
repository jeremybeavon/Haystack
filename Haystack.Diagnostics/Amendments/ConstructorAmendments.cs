using System;
using System.Collections.Generic;
using System.Linq;

namespace Haystack.Diagnostics.Amendments
{
    public static class ConstructorAmendments<TInstance>
    {
        public static void BeforeConstructor(TInstance instance, string constructor, object[] parameters)
        {
            ProcessConstructor(
                AmendmentRepository.BeforeConstructorAmenders,
                parameters,
                amendment => amendment.BeforeConstructor(instance, parameters));
        }

        public static void AfterConstructor(TInstance instance, string constructor, object[] parameters)
        {
            ProcessConstructor(
                AmendmentRepository.AfterConstructorAmenders,
                parameters,
                amendment => amendment.AfterConstructor(instance, parameters));
        }

        public static void CatchConstructor(TInstance instance, string constructor, object[] parameters)
        {
            ProcessConstructor(
                AmendmentRepository.CatchConstructorAmenders,
                parameters,
                amendment => amendment.CatchConstructor(instance, parameters));
        }

        private static void ProcessConstructor<TAmender>(
            IEnumerable<TAmender> amenders,
            object[] parameters,
            Action<TAmender> action)
            where TAmender : IConstructorAmender
        {
            if (amenders != null)
            {
                foreach (TAmender amendment in amenders.Where(amender => amender.AmendConstructor(typeof(TInstance), parameters)))
                {
                    action(amendment);
                }
            }
        }
    }
}
