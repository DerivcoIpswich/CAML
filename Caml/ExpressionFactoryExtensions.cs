using Caml.Ast;
using Caml.Expressions;
using Caml.Parser;

namespace Caml
{
    public static class ExpressionFactoryExtensions
    {
        public static IExpression BuildExpression(this ExpressionFactory expressionFactory, string pattern)
        {
            var tokens = new Lexer(new TokenFactory()).Tokenise(pattern);
            var symbol = new SymbolParser().Parse(tokens);

            return expressionFactory.BuildExpression(symbol);
        }

        public static IExpression BuildDefaultExpression(this ExpressionFactory expressionFactory)
        {
            var tokenFactory = new TokenFactory();
            
            var tokens = new[] {
                tokenFactory.BuildToken(TokenKind.Default),
                tokenFactory.BuildEnd()
            };

            var symbol = new SymbolParser().Parse(tokens);
            return expressionFactory.BuildExpression(symbol);
        }
    }
}