namespace Caml.Ast.Symbols
{
    public class UnaryPrefixOperation : ISymbol
    {
        public SymbolKind Kind => SymbolKind.UnaryPrefixOperation;

        public virtual Token Operator { get; internal set; }

        public virtual ISymbol Expression { get; internal set; }

        public override string ToString() => $"{Operator}{Expression}";
    }
}
