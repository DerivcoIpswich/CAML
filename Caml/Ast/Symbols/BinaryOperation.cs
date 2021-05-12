namespace Caml.Ast.Symbols
{
    public class BinaryOperation : ISymbol
    {
        public SymbolKind Kind => SymbolKind.BinaryOperation;

        public virtual Token Operator { get; internal set; }

        public virtual ISymbol Left { get; internal set; }

        public virtual ISymbol Right { get; internal set; }

        public override string ToString() => $"{Left} {Operator} {Right}";
    }
}
