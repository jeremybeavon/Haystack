using System;
using System.Collections.Generic;
using System.Linq;
using Haystack.Diagnostics.Configuration;

namespace Haystack.Diagnostics.Amendments
{
    internal sealed class AmendmentConfigurationBuilder
    {
        private readonly AmendmentConfiguration configuration;
        private readonly Lazy<IEnumerable<HaystackPropertyAmender>> propertyAmenders;
        private readonly Lazy<IEnumerable<HaystackConstructorAmender>> constructorAmenders;
        private readonly Lazy<IEnumerable<IMethodAmender>> methodAmenders;
        private readonly Lazy<IEnumerable<IBeforeMethodAmender>> beforeMethodAmenders;
        private readonly Lazy<IEnumerable<HaystackVoidMethodAmender>> voidMethodAmenders;
        private readonly Lazy<IEnumerable<HaystackNonVoidMethodAmender>> nonVoidMethodAmenders;

        public AmendmentConfigurationBuilder(AmendmentConfiguration configuration)
        {
            this.configuration = configuration;
            propertyAmenders = new Lazy<IEnumerable<HaystackPropertyAmender>>(BuildHaystackPropertyAmenders);
            constructorAmenders = new Lazy<IEnumerable<HaystackConstructorAmender>>(BuildHaystackConstructorAmenders);
            methodAmenders = new Lazy<IEnumerable<IMethodAmender>>(BuildMethodAmenders);
            beforeMethodAmenders = new Lazy<IEnumerable<IBeforeMethodAmender>>(BuildHaystackMethodAmenders);
            voidMethodAmenders = new Lazy<IEnumerable<HaystackVoidMethodAmender>>(BuildHaystackVoidMethodAmenders);
            nonVoidMethodAmenders = new Lazy<IEnumerable<HaystackNonVoidMethodAmender>>(BuildHaystackNonVoidMethodAmenders);
        }

        public IEnumerable<IBeforePropertyGetAmender> BuildBeforePropertyGetAmenders()
        {
            return CreateInstances<IBeforePropertyGetAmender>(configuration.BeforePropertyGetAmendments, propertyAmenders.Value);
        }

        public IEnumerable<IAfterPropertyGetAmender> BuildAfterPropertyGetAmenders()
        {
            return CreateInstances<IAfterPropertyGetAmender>(configuration.AfterPropertyGetAmendments, propertyAmenders.Value);
        }

        public IEnumerable<IBeforePropertySetAmender> BuildBeforePropertySetAmenders()
        {
            return CreateInstances<IBeforePropertySetAmender>(configuration.BeforePropertySetAmendments, propertyAmenders.Value);
        }

        public IEnumerable<IAfterPropertySetAmender> BuildAfterPropertySetAmenders()
        {
            return CreateInstances<IAfterPropertySetAmender>(configuration.AfterPropertySetAmendments, propertyAmenders.Value);
        }

        public IEnumerable<IBeforeConstructorAmender> BuildBeforeConstructorAmenders()
        {
            return CreateInstances<IBeforeConstructorAmender>(configuration.BeforeConstructorAmendments, constructorAmenders.Value);
        }

        public IEnumerable<IAfterConstructorAmender> BuildAfterConstructorAmenders()
        {
            return CreateInstances<IAfterConstructorAmender>(configuration.AfterConstructorAmendments, constructorAmenders.Value);
        }

        public IEnumerable<ICatchConstructorAmender> BuildCatchConstructorAmenders()
        {
            return TypeResolver.CreateInstances<ICatchConstructorAmender>(configuration.CatchConstructorAmendments);
        }

        public IEnumerable<IBeforeMethodAmender> BuildBeforeMethodAmenders()
        {
            return CreateInstances(configuration.BeforeMethodAmendments, beforeMethodAmenders.Value);
        }

        public IEnumerable<IAfterVoidMethodAmender> BuildAfterVoidMethodAmenders()
        {
            return CreateInstances<IAfterVoidMethodAmender>(configuration.AfterVoidMethodAmendments, voidMethodAmenders.Value);
        }

        public IEnumerable<IAfterMethodAmender> BuildAfterMethodAmenders()
        {
            return CreateInstances<IAfterMethodAmender>(configuration.AfterMethodAmendments, nonVoidMethodAmenders.Value);
        }

        public IEnumerable<ICatchVoidMethodAmender> BuildCatchVoidMethodAmenders()
        {
            return CreateInstances<ICatchVoidMethodAmender>(configuration.CatchVoidMethodAmendments, voidMethodAmenders.Value);
        }

        public IEnumerable<ICatchMethodAmender> BuildCatchMethodAmenders()
        {
            return CreateInstances<ICatchMethodAmender>(configuration.CatchMethodAmendments, nonVoidMethodAmenders.Value);
        }

        public IEnumerable<IFinallyMethodAmender> BuildFinallyMethodAmenders()
        {
            return TypeResolver.CreateInstances<IFinallyMethodAmender>(configuration.FinallyMethodAmendments);
        }

        private static IEnumerable<T> CreateInstances<T>(IEnumerable<TypeConfiguration> types, IEnumerable<T> extraAmenders)
            where T : class
        {
            return TypeResolver.CreateInstances<T>(types).Concat(extraAmenders).ToArray();
        }
        
        private IEnumerable<HaystackPropertyAmender> BuildHaystackPropertyAmenders()
        {
            return TypeResolver.CreateInstances<IPropertyAmender>(configuration.HaystackPropertyAmendments)
                .Select(amender => new HaystackPropertyAmender(amender))
                .ToArray();
        }

        private IEnumerable<HaystackConstructorAmender> BuildHaystackConstructorAmenders()
        {
            return TypeResolver.CreateInstances<IConstructorAmender>(configuration.HaystackConstructorAmendments)
                .Select(amender => new HaystackConstructorAmender(amender))
                .ToArray();
        }

        private IEnumerable<IMethodAmender> BuildMethodAmenders()
        {
            return TypeResolver.CreateInstances<IMethodAmender>(configuration.HaystackMethodAmendments);
        }

        private IEnumerable<IBeforeMethodAmender> BuildHaystackMethodAmenders()
        {
            return methodAmenders.Value.Select(amender => new HaystackBeforeMethodAmender(amender)).ToArray();
        }

        private IEnumerable<HaystackVoidMethodAmender> BuildHaystackVoidMethodAmenders()
        {
            return methodAmenders.Value.Select(amender => new HaystackVoidMethodAmender(amender)).ToArray();
        }

        private IEnumerable<HaystackNonVoidMethodAmender> BuildHaystackNonVoidMethodAmenders()
        {
            return methodAmenders.Value.Select(amender => new HaystackNonVoidMethodAmender(amender)).ToArray();
        }
    }
}
