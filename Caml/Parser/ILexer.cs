using System.Collections.Generic;
using Caml.Ast;

namespace Caml.Parser
{
    public interface ILexer
    {
        IEnumerable<Token> Tokenise(IEnumerable<char> expression);
    }
}