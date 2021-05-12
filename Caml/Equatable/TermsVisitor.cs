using Caml.Expressions;
using System.Collections.Generic;

namespace Caml.Equatable
{
    internal class TermsVisitor : ExpressionWalker
    {
        private readonly HashSet<string> terms;

        public TermsVisitor()
        {
            terms = new HashSet<string>();
        }

        internal IEnumerable<string> GetTerms(IExpression expression)
        {
            terms.Clear();
            Visit(expression);
            return terms;
        }

        public override void VisitLiteralExpression(LiteralExpression expression)
        {
            terms.Add(expression.ToString());
            base.VisitLiteralExpression(expression);
        }

        public override void VisitFunctionExpression(FunctionExpression expression)
        {
            terms.Add(expression.ToString());
            base.VisitFunctionExpression(expression);
        }
    }

}
