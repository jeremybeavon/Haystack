using Haystack.Diagnostics.Configuration;
using System.Collections.Generic;

namespace Haystack.Diagnostics.Amendments
{
    public static class AmendmentRepository
    {
        public static IEnumerable<IBeforePropertyGetAmender> BeforePropertyGetAmenders { get; set; }

        public static IEnumerable<IAfterPropertyGetAmender> AfterPropertyGetAmenders { get; set; }

        public static IEnumerable<IBeforePropertySetAmender> BeforePropertySetAmenders { get; set; }

        public static IEnumerable<IAfterPropertySetAmender> AfterPropertySetAmenders { get; set; }

        public static IEnumerable<IBeforeConstructorAmender> BeforeConstructorAmenders { get; set; }

        public static IEnumerable<IAfterConstructorAmender> AfterConstructorAmenders { get; set; }

        public static IEnumerable<ICatchConstructorAmender> CatchConstructorAmenders { get; set; }

        public static IEnumerable<IBeforeMethodAmender> BeforeMethodAmenders { get; set; }

        public static IEnumerable<IAfterVoidMethodAmender> AfterVoidMethodAmenders { get; set; }

        public static IEnumerable<IAfterMethodAmender> AfterMethodAmenders { get; set; }

        public static IEnumerable<ICatchVoidMethodAmender> CatchVoidMethodAmenders { get; set; }

        public static IEnumerable<ICatchMethodAmender> CatchMethodAmenders { get; set; }

        public static IEnumerable<IFinallyMethodAmender> FinallyMethodAmenders { get; set; }

        public static void Initialize(string configurationText)
        {
            IHaystackConfiguration configuration = HaystackConfiguration.LoadText(configurationText);
            Initialize(configuration.Amendments);
        }

        public static void Initialize(IAmendmentConfiguration configuration)
        {
            BeforePropertyGetAmenders = configuration.BeforePropertyGetAmendments;
            AfterPropertyGetAmenders = configuration.AfterPropertyGetAmendments;
            BeforePropertySetAmenders = configuration.BeforePropertySetAmendments;
            AfterPropertySetAmenders = configuration.AfterPropertySetAmendments;
            BeforeConstructorAmenders = configuration.BeforeConstructorAmendments;
            CatchConstructorAmenders = configuration.CatchConstructorAmendments;
            AfterConstructorAmenders = configuration.AfterConstructorAmendments;
            BeforeMethodAmenders = configuration.BeforeMethodAmendments;
            AfterVoidMethodAmenders = configuration.AfterVoidMethodAmendments;
            AfterMethodAmenders = configuration.AfterMethodAmendments;
            CatchVoidMethodAmenders = configuration.CatchVoidMethodAmendments;
            CatchMethodAmenders = configuration.CatchMethodAmendments;
            FinallyMethodAmenders = configuration.FinallyMethodAmendments;
        }
    }
}
