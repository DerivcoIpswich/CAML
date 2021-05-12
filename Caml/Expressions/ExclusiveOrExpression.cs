using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Equatable;

namespace Caml.Expressions
{
    public class ExclusiveOrExpression : IExpression, IBinaryExpression
    {
        ISymbol IExpression.Symbol => Symbol;

        public BinaryOperation Symbol { get; }

        public IExpression Left { get; }

        public IExpression Right { get; }

        public ExclusiveOrExpression(BinaryOperation symbol, IExpression left, IExpression right)
        {
            Symbol = symbol;
            Left = left;
            Right = right;
        }

        public override string ToString() => Symbol.ToString();

        public bool IsMatch(IMatcher matcher)
        {
            return Left.IsMatch(matcher) ^ Right.IsMatch(matcher);
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitBinaryExpression(this);
        }

        public bool Equals(IExpression other)
        {
            return new TruthTable(this).Equals(new TruthTable(other));
        }
    }
}