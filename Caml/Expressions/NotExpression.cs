using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Equatable;

namespace Caml.Expressions
{
    public class NotExpression : IExpression, IUnaryExpression
    {
        ISymbol IExpression.Symbol => Symbol;

        public UnaryPrefixOperation Symbol { get; }

        public IExpression Expression { get; }

        public NotExpression(UnaryPrefixOperation symbol, IExpression expression)
        {
            Symbol = symbol;
            Expression = expression;
        }

        public override string ToString() => Symbol.ToString();

        public bool IsMatch(IMatcher matcher)
        {
            return !Expression.IsMatch(matcher);
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitUnaryExpression(this);
        }

        public bool Equals(IExpression other)
        {
            return new TruthTable(this).Equals(new TruthTable(other));
        }
    }
}