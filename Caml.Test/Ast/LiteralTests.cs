using Caml.Ast.Symbols;
using Xunit;

namespace Caml.Ast
{
    public class LiteralTests
    {
        new public class ToString
        {
            [Fact]
            public void returns_token_value()
            {
                //Arrange
                var subject = new Literal
                {
                    Token = new Token("test", TokenKind.Literal)
                };

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
