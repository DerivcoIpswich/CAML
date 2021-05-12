namespace Caml.Ast.Symbols
{
    public class Default : ISymbol
    {
        public SymbolKind Kind => SymbolKind.Default;

        public virtual Token Token { get; internal set; }

        public override string ToString() => Token.ToString();
    }
}
