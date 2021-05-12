using Caml.Ast.Symbols;
using Moq;
using Xunit;

namespace Caml.Ast
{
    public class BinaryOperationTests
    {
        new public class ToString
        {
            [Fact]
            public void returns_expression_prefixed_with_operator()
            {
                //Arrange
                var left = new Mock<ISymbol>();
                left.Setup(e => e.ToString()).Returns("left");
                var right = new Mock<ISymbol>();
                right.Setup(e => e.ToString()).Returns("right");
                var subject = new BinaryOperation
                {
                    Left = left.Object,
                    Operator = new Token("%", TokenKind.And),
                    Right = right.Object
                };

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("left % right", result);
            }
        }
    }
}
