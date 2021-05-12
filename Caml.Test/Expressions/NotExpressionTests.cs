using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using Xunit;

namespace Caml.Expressions
{
    public class NotExpressionTests
    {
        public class IsMatch
        {
            [Fact]
            public void returns_true_if_expression_is_false()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(expressionReturns: false);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void returns_false_if_expression_is_true()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(expressionReturns: true);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.False(result);
            }

            private static NotExpression BuildSubject(bool expressionReturns)
            {
                var symbol = Mock.Of<UnaryPrefixOperation>();
                var expressionMock = new Mock<IExpression>();
                expressionMock.Setup(m => m.IsMatch(It.IsAny<IMatcher>())).Returns(expressionReturns);
                return new NotExpression(symbol, expressionMock.Object);
            }
        }

        new public class ToString
        {
            [Fact]
            public void returns_string_from_symbol()
            {
                //Arrange
                var symbol = new Mock<UnaryPrefixOperation>();
                symbol.Setup(s => s.ToString()).Returns("test");
                var subject = new NotExpression(symbol.Object, null);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
