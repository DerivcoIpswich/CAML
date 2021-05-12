using Caml.Ast;
using Caml.Ast.Symbols;

namespace Caml.Expressions
{
    public interface IGroupedExpression : IExpression
    {
        Token Open { get; }

        IExpression Expression { get; }
        
        Token Close { get; }

        new ExpressionGroup Symbol { get; }
    }
}