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
    }
}
