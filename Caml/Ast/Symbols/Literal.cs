namespace Caml.Ast.Symbols
{
    public class Literal : ISymbol
    {
        public SymbolKind Kind => SymbolKind.Literal;

        public virtual Token Token { get; internal set; }

        public override string ToString() => Token.ToString();
    }
}
