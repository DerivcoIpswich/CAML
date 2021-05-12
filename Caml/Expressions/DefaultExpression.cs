using Caml.Ast;
using Caml.Ast.Symbols;

namespace Caml.Expressions
{
    public class DefaultExpression : IExpression
    {
        ISymbol IExpression.Symbol => Symbol;

        public Default Symbol { get; }
        
        public DefaultExpression(Default symbol)
        {
            Symbol = symbol;
        }

        public bool IsMatch(IMatcher matcher) => true;

        public override string ToString() => Symbol.ToString();

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitDefaultExpression(this);
        }
        public bool Equals(IExpression other)
        {
            return other is DefaultExpression;
        }
    }
}