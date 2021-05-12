namespace Caml.Ast.Symbols
{
    public interface IExpressionGroup
    {
        Token Close { get; }
        ISymbol Expression { get; }
        Token Open { get; }
    }
}