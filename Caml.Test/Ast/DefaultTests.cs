using Caml.Ast.Symbols;
using Xunit;

namespace Caml.Ast
{
    public class DefaultTests
    {
        new public class ToString
        {
            [Fact]
            public void returns_token_value()
            {
                //Arrange
                var subject = new Default
                {
                    Token = new Token("test", TokenKind.Default)
                };

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
