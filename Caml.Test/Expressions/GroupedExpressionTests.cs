using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using Xunit;

namespace Caml.Expressions
{
    public class GroupedExpressionTests
    {
        public class IsMatch
        {
            [Fact]
            public void returns_true_if_expression_is_true()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(expressionReturns: true);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void returns_false_if_expression_is_false()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(expressionReturns: false);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.False(result);
            }

            private static GroupedExpression BuildSubject(bool expressionReturns)
            {
                var symbol = Mock.Of<ExpressionGroup>();
                var expressionMock = new Mock<IExpression>();
                expressionMock.Setup(m => m.IsMatch(It.IsAny<IMatcher>())).Returns(expressionReturns);
                return new GroupedExpression(symbol, expressionMock.Object);
            }
        }

        new public class ToString
        {
            [Fact]
            public void returns_string_from_expression_with_open_and_close_tokens()
            {
                //Arrange
                var symbol = new Mock<ExpressionGroup>();
                symbol.Setup(s => s.Open).Returns(new Token("<", TokenKind.OpenParenthesis));
                symbol.Setup(s => s.Close).Returns(new Token(">", TokenKind.CloseParenthesis));
                var expressionMock = new Mock<IExpression>();
                expressionMock.Setup(m => m.ToString()).Returns("test");

                var subject = new GroupedExpression(symbol.Object, expressionMock.Object);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("<test>", result);
            }
        }
    }
}
