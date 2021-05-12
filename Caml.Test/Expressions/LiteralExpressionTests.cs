using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using Xunit;

namespace Caml.Expressions
{
    public class LiteralExpressionTests
    {
        public class IsMatch
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void returns_result_from_matcher(bool matcherReturns)
            {
                //Arrange
                var matcher = new Mock<IMatcher>();
                matcher.Setup(m => m.Match("literal")).Returns(matcherReturns).Verifiable();
                var symbol = new Mock<Literal>();
                symbol.Setup(s => s.Token).Returns(new Token("literal", TokenKind.Literal));
                var subject = new LiteralExpression(symbol.Object);

                //Act
                var result = subject.IsMatch(matcher.Object);

                //Assert
                matcher.Verify();
                Assert.Equal(matcherReturns, result);
            }
        }

        new public class ToString
        {
            [Fact]
            public void returns_string_from_symbol()
            {
                //Arrange
                var symbol = new Mock<Literal>();
                symbol.Setup(s => s.ToString()).Returns("test");
                var subject = new LiteralExpression(symbol.Object);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
