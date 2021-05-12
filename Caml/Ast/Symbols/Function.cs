using System.Collections.Generic;

namespace Caml.Ast.Symbols
{
    public class Function : ISymbol
    {
        public SymbolKind Kind => SymbolKind.Function;

        public virtual Token Name { get; internal set; }

        public virtual Token Open { get; internal set; }

        public virtual IReadOnlyList<Literal> Arguments { get; internal set; }

        public virtual Token Close { get; internal set; }

        public override string ToString()
        {
            return $"{Name.Value}{Open}{string.Join(", ", Arguments)}{Close}";
        }
    }
}
