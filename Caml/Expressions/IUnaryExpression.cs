using Caml.Ast.Symbols;

namespace Caml.Expressions
{
    public interface IUnaryExpression : IExpression
    {
        IExpression Expression { get; }

        new UnaryPrefixOperation Symbol { get; }
    }
}