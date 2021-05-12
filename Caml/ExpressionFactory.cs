using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Expressions;
using System;

namespace Caml
{
    public class ExpressionFactory
    {
        public IExpression BuildExpression(ISymbol symbol)
        {
            if(symbol is null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            switch (symbol.Kind)
            {
                case SymbolKind.Default:
                    return new DefaultExpression(symbol as Default);
                case SymbolKind.Literal:
                    return new LiteralExpression(symbol as Literal);
                case SymbolKind.BinaryOperation:
                    return CreateBinaryExpression(symbol as BinaryOperation);
                case SymbolKind.UnaryPrefixOperation:
                    return CreateUnaryExpression(symbol as UnaryPrefixOperation);
                case SymbolKind.Function:
                    return new FunctionExpression(symbol as Function);
                case SymbolKind.ExpressionGroup:
                    return CreateExpressionGroup(symbol as ExpressionGroup);
                default:
                    throw new InvalidOperationException();
            }
        }

        private IBinaryExpression CreateBinaryExpression(BinaryOperation symbol)
        {
            var left = BuildExpression(symbol.Left);
            var right = BuildExpression(symbol.Right);

            return CreateBinaryExpression(symbol, left, right);
        }

        private static IBinaryExpression CreateBinaryExpression(BinaryOperation symbol, IExpression left, IExpression right)
        {
            switch (symbol.Operator.Kind)
            {
                case TokenKind.And:
                    return new AndExpression(symbol, left, right);
                case TokenKind.Or:
                    return new OrExpression(symbol, left, right);
                case TokenKind.XOr:
                    return new ExclusiveOrExpression(symbol, left, right);
                default:
                    throw new InvalidOperationException();
            }
        }

        public IBinaryExpression BuildBinaryExpression(IExpression left, Token op, IExpression right)
        {
            if(left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            var symbol = new BinaryOperation
            {
                Left = left.Symbol,
                Right = right.Symbol,
                Operator = op
            };

            return CreateBinaryExpression(symbol, left, right);
        }

        private IExpression CreateUnaryExpression(UnaryPrefixOperation symbol)
        {
            var expression = BuildExpression(symbol.Expression);

            switch (symbol.Operator.Kind)
            {
                case TokenKind.Not:
                    return new NotExpression(symbol, expression);
                default:
                    throw new InvalidOperationException();
            }
        }

        public IUnaryExpression BuildUnaryPrefixExpression(Token op, IExpression expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var symbol = new UnaryPrefixOperation
            {
                Expression = expression.Symbol,
                Operator = op
            };

            switch (symbol.Operator.Kind)
            {
                case TokenKind.Not:
                    return new NotExpression(symbol, expression);
                default:
                    throw new InvalidOperationException();
            }
        }

        private IExpression CreateExpressionGroup(ExpressionGroup symbol)
        {
            var expression = BuildExpression(symbol.Expression);

            return new GroupedExpression(symbol, expression);
        }

        public IGroupedExpression BuildGroupedExpression(Token open, IExpression expression, Token close)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var symbol = new ExpressionGroup
            {
                Open = open,
                Expression = expression.Symbol,
                Close = close,
            };

            return new GroupedExpression(symbol, expression);
        }
    }
}