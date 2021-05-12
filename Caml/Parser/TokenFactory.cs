using Caml.Ast;
using System;
using System.Linq;

namespace Caml.Parser
{
    public class TokenFactory : ITokenFactory
    {
        private static readonly char[] boundries = new[] { '&', '|', '(', ')', ',', '!', '^' };
        private static readonly char[] reserved = new[] { '$' };

        public Token BuildToken(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName: nameof(value));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message: $"{nameof(value)} cannot be whitespace or empty", paramName: nameof(value));
            }

            switch (value)
            {
                case "default":
                    return BuildToken(TokenKind.Default);
                case "&":
                    return BuildToken(TokenKind.And);
                case "|":
                    return BuildToken(TokenKind.Or);
                case "^":
                    return BuildToken(TokenKind.XOr);
                case "!":
                    return BuildToken(TokenKind.Not);
                case "(":
                    return BuildToken(TokenKind.OpenParenthesis);
                case ")":
                    return BuildToken(TokenKind.CloseParenthesis);
                case ",":
                    return BuildToken(TokenKind.Comma);
                default:
                    return new Token(value, TokenKind.Literal);
            }
        }

        public Token BuildToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.Default:
                    return new Token("default", tokenKind);                
                case TokenKind.And:
                    return new Token("&", tokenKind);
                case TokenKind.XOr:
                    return new Token("^", tokenKind);
                case TokenKind.Or:
                    return new Token("|", tokenKind);
                case TokenKind.Not:
                    return new Token("!", tokenKind);
                case TokenKind.OpenParenthesis:
                    return new Token("(", tokenKind);
                case TokenKind.CloseParenthesis:
                    return new Token(")", tokenKind);
                case TokenKind.Comma:
                    return new Token(",", tokenKind);
                case TokenKind.End:
                    return new Token("$", tokenKind);
                case TokenKind.Literal:
                    throw new ArgumentException("cannot build literal token without value", paramName:nameof(tokenKind));
                default:
                    throw new ArgumentException($"invalid {nameof(TokenKind)}", paramName: nameof(tokenKind));
            }
        }

        public Token BuildEnd()
        {
            return BuildToken(TokenKind.End);
        }

        public bool IsBoundry(char value) => boundries.Contains(value) || char.IsWhiteSpace(value);


        public bool IsReserved(char current) => reserved.Contains(current);


        public bool IsTrivia(char current) => char.IsWhiteSpace(current);
    }
}
