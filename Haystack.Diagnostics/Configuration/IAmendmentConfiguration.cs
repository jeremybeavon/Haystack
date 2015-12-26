using Haystack.Diagnostics.Amendments;
using System.Collections.Generic;

namespace Haystack.Diagnostics.Configuration
{
    public interface IAmendmentConfiguration
    {
        IEnumerable<string> Assemblies { get; }

        IEnumerable<IPropertyAmender> HaystackPropertyAmendments { get; }

        IEnumerable<IConstructorAmender> HaystackConstructorAmendments { get; }

        IEnumerable<IMethodAmender> HaystackMethodAmendments { get; }

        IEnumerable<IBeforePropertyGetAmender> BeforePropertyGetAmendments { get; }

        IEnumerable<IAfterPropertyGetAmender> AfterPropertyGetAmendments { get; }

        IEnumerable<IBeforePropertyGetAmender> BeforePropertySetAmendments { get; }

        IEnumerable<IAfterPropertySetAmender> AfterPropertySetAmendments { get; }

        IEnumerable<IBeforeConstructorAmender> BeforeConstructorAmendments { get; }

        IEnumerable<IAfterConstructorAmender> AfterConstructorAmendments { get; }

        IEnumerable<ICatchConstructorAmender> CatchConstructorAmendments { get; }

        IEnumerable<IBeforeMethodAmender> BeforeMethodAmendments { get; }

        IEnumerable<IAfterVoidMethodAmender> AfterVoidMethodAmendments { get; }

        IEnumerable<IAfterMethodAmender> AfterMethodAmendments { get; }

        IEnumerable<ICatchVoidMethodAmender> CatchVoidMethodAmendments { get; }

        IEnumerable<ICatchMethodAmender> CatchMethodAmendments { get; }

        IEnumerable<IFinallyMethodAmender> FinallyMethodAmendments { get; }
    }
}
