using Caml.Ast.Symbols;
using Moq;
using Xunit;

namespace Caml.Ast
{
    public class ExpressionGroupTests
    {
        new public class ToString
        {
            [Fact]
            public void returns_expression_surrounded_by_parentheses()
            {
                //Arrange
                var expression = new Mock<ISymbol>();
                expression.Setup(e => e.ToString()).Returns("test");
                var subject = new ExpressionGroup
                {
                    Open = new Token("{", TokenKind.OpenParenthesis),
                    Expression  = expression.Object,
                    Close = new Token("}", TokenKind.CloseParenthesis),
                };

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("{test}", result);
            }
        }
    }
}
