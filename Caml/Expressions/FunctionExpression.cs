using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Equatable;
using System;
using System.Linq;

namespace Caml.Expressions
{
    public class FunctionExpression : IExpression
    {
        ISymbol IExpression.Symbol => Symbol;

        public Function Symbol { get; }

        public FunctionExpression(Function symbol)
        {
            Symbol = symbol;
        }

        public override string ToString() => Symbol.ToString();

        public bool IsMatch(IMatcher matcher)
        {
            return matcher.Match(Symbol.Name.Value, Symbol.Arguments.Select(a => a.Token.Value).ToArray());
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitFunctionExpression(this);
        }

        public bool Equals(IExpression other)
        {
            return new TruthTable(this).Equals(new TruthTable(other));
        }
    }
}