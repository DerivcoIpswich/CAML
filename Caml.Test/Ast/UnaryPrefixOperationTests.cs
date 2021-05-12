using Caml.Ast.Symbols;
using Moq;
using Xunit;

namespace Caml.Ast
{
    public class UnaryPrefixOperationTests
    {
        new public class ToString
        {
            [Fact]
            public void returns_expression_prefixed_with_operator()
            {
                //Arrange
                var expression = new Mock<ISymbol>();
                expression.Setup(e => e.ToString()).Returns("test");
                var subject = new UnaryPrefixOperation
                {
                    Expression = expression.Object,
                    Operator = new Token("%", TokenKind.Not)
                };

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("%test", result);
            }
        }
    }
}
