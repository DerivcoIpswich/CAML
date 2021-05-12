using Caml.Ast;

namespace Caml.Parser
{
    public interface ITokenFactory
    {
        Token BuildEnd();

        Token BuildToken(string value);
        Token BuildToken(TokenKind tokenKind);
        bool IsBoundry(char value);

        bool IsReserved(char current);

        bool IsTrivia(char current);
    }
}