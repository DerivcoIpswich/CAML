using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Equatable;

namespace Caml.Expressions
{
    public class LiteralExpression : IExpression
    {
        ISymbol IExpression.Symbol => Symbol;

        private Literal Symbol { get; }

        public LiteralExpression(Literal symbol)
        {
            Symbol = symbol;
        }

        public override string ToString() => Symbol.ToString();

        public bool IsMatch(IMatcher matcher) => matcher.Match(Symbol.Token.Value);

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitLiteralExpression(this);
        }

        public bool Equals(IExpression other)
        {
            return new TruthTable(this).Equals(new TruthTable(other));
        }
    }
}