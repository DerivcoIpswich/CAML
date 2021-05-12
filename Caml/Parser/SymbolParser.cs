using Caml.Ast;
using Caml.Ast.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Caml.Parser
{
    public class SymbolParser
    {
        private Queue<Token> queue;

        public ISymbol Parse(IEnumerable<Token> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(paramName: nameof(tokens));
            }
            if (!tokens.Any())
            {
                throw new ArgumentException(message: "cannot parse empty expression", paramName: nameof(tokens));
            }

            queue = new Queue<Token>(tokens);

            switch (PeekKind())
            {
                case TokenKind.End:
                    throw new ArgumentException("cannot parse empty expression");
                case TokenKind.Default:
                    var defaultSymbol = ParseDefault();
                    Eat(TokenKind.End);
                    return defaultSymbol;
                default:
                    var symbol = Parse();
                    Eat(TokenKind.End);
                    return symbol;
            }
        }

        private ISymbol Parse()
        {
            switch (PeekKind())
            {
                case TokenKind.Literal:
                    return ParseLiteral();
                case TokenKind.OpenParenthesis:
                    return ParseExpressionGroup();
                case TokenKind.Not:
                    return ParseUnaryPrefixOperation();
                default:
                    throw new InvalidOperationException($"unexpected token: '{Eat()}'");
            }
        }

        private ISymbol ParseLiteral() => ParseLiteral(s => s);

        private ISymbol ParseLiteral(Func<ISymbol, ISymbol> prefixProcessor = null)
        {
            Token token = Eat(TokenKind.Literal);

            switch (PeekKind())
            {
                case TokenKind.End:
                case TokenKind.CloseParenthesis:
                    return prefixProcessor(new Literal { Token = token });
                case TokenKind.And:
                case TokenKind.Or:
                case TokenKind.XOr:
                    return ParseBinaryOperation(prefixProcessor(new Literal { Token = token }));
                case TokenKind.OpenParenthesis:
                    return ParseFunction(token, prefixProcessor);
                default:
                    throw new InvalidOperationException($"unexpected token after literal '{token}': '{Eat()}'");
            }
        }

        private ISymbol ParseFunction(Token functionName, Func<ISymbol, ISymbol> prefixProcessor)
        {
            var function = new Function
            {
                Name = functionName,
                Open = Eat(TokenKind.OpenParenthesis),
                Arguments = ParseArguments().ToList(),
                Close = Eat(TokenKind.CloseParenthesis)
            };

            switch (PeekKind())
            {
                case TokenKind.End:
                case TokenKind.CloseParenthesis:
                    return prefixProcessor(function);
                case TokenKind.And:
                case TokenKind.Or:
                case TokenKind.XOr:
                    return ParseBinaryOperation(prefixProcessor(function));
                default:
                    throw new InvalidOperationException($"unexpected token after function '{function}': '{Eat()}'");
            }
        }

        private IEnumerable<Literal> ParseArguments()
        {
            while (PeekKind() != TokenKind.CloseParenthesis)
            {
                yield return ParseArguemnt();
            }
        }

        private Literal ParseArguemnt()
        {
            var token = Eat(TokenKind.Literal);
            var literal = new Literal { Token = token };

            switch (PeekKind())
            {
                case TokenKind.CloseParenthesis:
                    return literal;
                case TokenKind.Comma:
                    Eat(TokenKind.Comma);
                    if (PeekKind() == TokenKind.CloseParenthesis)
                    {
                        throw new InvalidOperationException($"unexpected token after argument '{literal}': '{Eat()}'");
                    }
                    return literal;
                default:
                    throw new InvalidOperationException($"unexpected token after argument '{literal}': '{Eat()}'");
            }
        }

        private ISymbol ParseBinaryOperation(ISymbol left)
        {
            return new BinaryOperation
            {
                Left = left,
                Operator = Eat(TokenKind.And, TokenKind.Or, TokenKind.XOr),
                Right = Parse(),
            };
        }

        private ISymbol ParseDefault()
        {
            return new Default { Token = Eat(TokenKind.Default) };
        }

        private ISymbol ParseExpressionGroup()
        {
            var expressionGroup = new ExpressionGroup
            {
                Open = Eat(TokenKind.OpenParenthesis),
                Expression = Parse(),
                Close = Eat(TokenKind.CloseParenthesis)
            };

            switch (PeekKind())
            {
                case TokenKind.End:
                    return expressionGroup;
                case TokenKind.And:
                case TokenKind.Or:
                case TokenKind.XOr:
                    return ParseBinaryOperation(expressionGroup);
                default:
                    throw new InvalidOperationException($"unexpected token after expression group '{expressionGroup}': '{Eat()}'");
            }
        }

        private ISymbol ParseUnaryPrefixOperation()
        {
            var prefix = Eat(TokenKind.Not);

            switch (PeekKind())
            {
                case TokenKind.Literal:
                    return ParseLiteral(e => new UnaryPrefixOperation
                    {
                        Operator = prefix,
                        Expression = e,
                    });
                case TokenKind.OpenParenthesis:
                    return new UnaryPrefixOperation
                    {
                        Operator = prefix,
                        Expression = ParseExpressionGroup(),
                    };
                case TokenKind.Not:
                    return new UnaryPrefixOperation
                    {
                        Operator = prefix,
                        Expression = ParseUnaryPrefixOperation(),
                    };
                default:
                    throw new InvalidOperationException($"unexpected token: '{Eat()}'");
            }
        }

        private Token Eat()
        {
            return queue.Dequeue();
        }

        private Token Eat(params TokenKind[] kinds)
        {
            var token = Eat();

            if (kinds.Contains(token.Kind))
            {
                return token;
            }

            throw new InvalidOperationException($"expected token of kind {string.Join(" or ", kinds.Select(k => $"'{k}'"))}, but was '{token.Kind}'");
        }

        private TokenKind PeekKind()
        {
            return queue.Peek().Kind;
        }
    }
}
