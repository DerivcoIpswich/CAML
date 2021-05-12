using Caml.Expressions;
using System;
using System.Linq;

namespace Caml
{
    public class ExpressionValidator : ExpressionWalker
    {
        public Action<string> ValidateLiteral { get; set; }

        public Action<string, string[]> ValidateFunction { get; set; }

        public override void VisitFunctionExpression(FunctionExpression expression)
        {
            ValidateFunction?.Invoke(expression.Symbol.Name.ToString(), expression.Symbol.Arguments.Select(a => a.Token.Value).ToArray());
        }

        public override void VisitLiteralExpression(LiteralExpression expression)
        {
            ValidateLiteral?.Invoke(expression.ToString());
        }
    }
}
