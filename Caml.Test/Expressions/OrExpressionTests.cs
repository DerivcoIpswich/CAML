using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using Xunit;

namespace Caml.Expressions
{
    public class OrExpressionTests
    {
        public class IsMatch
        {
            [Fact]
            public void returns_true_if_both_operands_are_true()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(leftReturns: true, rightReturns: true);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void returns_true_if_either_operand_is_true(bool leftReturns, bool rightReturns)
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(leftReturns, rightReturns);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void returns_false_if_both_operands_are_false()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(leftReturns: false, rightReturns: false);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void evaluation_is_conditional()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var leftMock = MockExpression(true);
                var rightMock = MockExpression(true);
                var subject = BuildSubject(leftMock, rightMock);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                leftMock.Verify(m => m.IsMatch(matcher), Times.Once());
                rightMock.Verify(m => m.IsMatch(matcher), Times.Never());
            }

            private static OrExpression BuildSubject(bool leftReturns, bool rightReturns)
            {
                var leftMock = MockExpression(leftReturns);
                var rightMock = MockExpression(rightReturns);
                return BuildSubject(leftMock, rightMock);
            }

            private static OrExpression BuildSubject(Mock<IExpression> leftMock, Mock<IExpression> rightMock)
            {
                var symbol = Mock.Of<BinaryOperation>();
                return new OrExpression(symbol, leftMock.Object, rightMock.Object);
            }

            private static Mock<IExpression> MockExpression(bool returns)
            {
                var mock = new Mock<IExpression>();
                mock.Setup(m => m.IsMatch(It.IsAny<IMatcher>())).Returns(returns);
                return mock;
            }
        }

        new public class ToString
        {
            [Fact]
            public void returns_string_from_symbol()
            {
                //Arrange
                var symbol = new Mock<BinaryOperation>();
                symbol.Setup(s => s.ToString()).Returns("test");
                var subject = new OrExpression(symbol.Object, null, null);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
