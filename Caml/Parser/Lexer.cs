using Caml.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caml.Parser
{
    public class Lexer : ILexer
    {
        private readonly ITokenFactory tokenFactory;

        public Lexer(ITokenFactory tokenFactory)
        {
            if (tokenFactory is null)
            {
                throw new ArgumentNullException(nameof(tokenFactory));
            }

            this.tokenFactory = tokenFactory;
        }

        public IEnumerable<Token> Tokenise(IEnumerable<char> expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var buffer = new StringBuilder();
            using (var e = expression.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    var current = e.Current;
                    if (tokenFactory.IsReserved(current))
                    {
                        throw new ArgumentException($"Illegal use of reserved character: {current}");
                    }
                    if (tokenFactory.IsBoundry(current))
                    {
                        if (buffer.Length > 0)
                        {
                            yield return tokenFactory.BuildToken(buffer.ToString());
                            buffer.Clear();
                        }
                        if (!tokenFactory.IsTrivia(current))
                        {
                            yield return tokenFactory.BuildToken(current.ToString());
                        }
                    }
                    else if (!tokenFactory.IsTrivia(current))
                    {
                        buffer.Append(current);
                    }
                }
            }
            if (buffer.Length > 0)
            {
                yield return tokenFactory.BuildToken(buffer.ToString());
                buffer.Clear();
            }
            yield return tokenFactory.BuildEnd();
        }
    }
}
