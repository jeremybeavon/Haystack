using System.Linq;
using System.Linq.Expressions;

namespace Haystack.Diagnostics.Amendments.Amender
{
    internal sealed class ExpressionRewriter : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;
        private readonly string overrideValue;

        public ExpressionRewriter(ParameterExpression parameter, string overrideValue)
        {
            this.parameter = parameter;
            this.overrideValue = overrideValue;
        }

        public static LambdaExpression RewriteExpression(LambdaExpression expression, string overrideValue)
        {
            return (LambdaExpression)new ExpressionRewriter(expression.Parameters.Last(), overrideValue).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == parameter ? (Expression)Expression.Constant(overrideValue) : node;
        }
    }
}
