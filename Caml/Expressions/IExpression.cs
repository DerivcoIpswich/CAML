using Caml.Ast;
using Caml.Equatable;
using System;

namespace Caml.Expressions
{
    public interface IExpression : IEquatable<IExpression>
    {
        ISymbol Symbol { get; }

        bool IsMatch(IMatcher matcher);

        void Accept(IExpressionVisitor visitor);
    }
}