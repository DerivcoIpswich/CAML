using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using Xunit;

namespace Caml.Expressions
{
    public class ExclusiveOrExpressionExpressionTests
    {
        public class IsMatch
        {
            [Fact]
            public void returns_false_if_both_operands_are_true()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject(leftReturns: true, rightReturns: true);

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.False(result);
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

            private static ExclusiveOrExpression BuildSubject(bool leftReturns, bool rightReturns)
            {
                var symbol = Mock.Of<BinaryOperation>();
                var leftMock = new Mock<IExpression>();
                leftMock.Setup(m => m.IsMatch(It.IsAny<IMatcher>())).Returns(leftReturns);
                var rightMock = new Mock<IExpression>();
                rightMock.Setup(m => m.IsMatch(It.IsAny<IMatcher>())).Returns(rightReturns);
                return new ExclusiveOrExpression(symbol, leftMock.Object, rightMock.Object);
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
                var subject = new ExclusiveOrExpression(symbol.Object, null, null);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
