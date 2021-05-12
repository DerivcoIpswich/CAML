using Caml.Expressions;

namespace Caml
{
    public interface IExpressionVisitor
    {
        void Visit(IExpression expression);

        void VisitBinaryExpression(IBinaryExpression expression);

        void VisitUnaryExpression(IUnaryExpression expression);

        void VisitGroupedExpression(IGroupedExpression expression);

        void VisitLiteralExpression(LiteralExpression expression);

        void VisitFunctionExpression(FunctionExpression expression);

        void VisitDefaultExpression(DefaultExpression expression);
    }
}
