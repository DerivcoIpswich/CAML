using Caml.Expressions;

namespace Caml
{
    public abstract class ExpressionWalker : IExpressionVisitor
    {
        public void Visit(IExpression expression)
        {
            expression.Accept(this);
        }

        public virtual void VisitBinaryExpression(IBinaryExpression expression)
        {
            Visit(expression.Left);
            Visit(expression.Right);
        }

        public virtual void VisitUnaryExpression(IUnaryExpression expression)
        {
            Visit(expression.Expression);
        }

        public virtual void VisitGroupedExpression(IGroupedExpression expression)
        {
            Visit(expression.Expression);
        }

        public virtual void VisitLiteralExpression(LiteralExpression expression)
        {

        }

        public virtual void VisitFunctionExpression(FunctionExpression expression)
        {

        }

        public virtual void VisitDefaultExpression(DefaultExpression expression)
        {

        }
    }
}
