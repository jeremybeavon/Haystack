using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Haystack.Diagnostics.Amendments.Amender
{
    internal sealed class PropertyAmendment
    {
        public PropertyAmendment(PropertyInfo property)
        {
            Property = property;
            BeforePropertyGetExpressions = new List<LambdaExpression>();
            AfterPropertyGetExpressions = new List<LambdaExpression>();
            BeforePropertySetExpressions = new List<LambdaExpression>();
            AfterPropertySetExpressions = new List<LambdaExpression>();
        }

        public PropertyInfo Property { get; private set; }

        public IList<LambdaExpression> BeforePropertyGetExpressions { get; private set; }

        public IList<LambdaExpression> AfterPropertyGetExpressions { get; private set; }

        public IList<LambdaExpression> BeforePropertySetExpressions { get; private set; }

        public IList<LambdaExpression> AfterPropertySetExpressions { get; private set; }
    }
}
