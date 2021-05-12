namespace Caml.Ast.Symbols
{
    public class ExpressionGroup : ISymbol
    {
        public SymbolKind Kind => SymbolKind.ExpressionGroup;

        public virtual ISymbol Expression { get; internal set; }

        public virtual Token Open { get; internal set; }

        public virtual Token Close { get; internal set; }
        
        public override string ToString() => $"{Open}{Expression}{Close}";
    }
}
