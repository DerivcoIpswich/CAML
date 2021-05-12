using Caml.Ast.Symbols;

namespace Caml.Expressions
{
    public interface IBinaryExpression : IExpression
    {
        IExpression Left { get; }

        IExpression Right { get; }

        new BinaryOperation Symbol { get; }
    }
}