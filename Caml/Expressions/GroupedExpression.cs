using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Equatable;

namespace Caml.Expressions
{
    public class GroupedExpression : IExpression, IGroupedExpression
    {
        ISymbol IExpression.Symbol => Symbol;

        public ExpressionGroup Symbol { get; }

        public Token Open => Symbol.Open;

        public Token Close => Symbol.Close;

        public IExpression Expression { get; }

        public GroupedExpression(ExpressionGroup symbol, IExpression expression)
        {
            Symbol = symbol;
            Expression = expression;
        }

        public override string ToString() => $"{Open}{Expression}{Close}";

        public bool IsMatch(IMatcher matcher)
        {
            return Expression.IsMatch(matcher);
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitGroupedExpression(this);
        }

        public bool Equals(IExpression other)
        {
            return new TruthTable(this).Equals(new TruthTable(other));
        }
    }
}