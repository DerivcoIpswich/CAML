using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using Xunit;

namespace Caml.Expressions
{
    public class DefaultExpressionTests
    {
        public class IsMatch
        {
            [Fact]
            public void returns_true()
            {
                //Arrange
                var matcher = Mock.Of<IMatcher>();
                var subject = BuildSubject();

                //Act
                var result = subject.IsMatch(matcher);

                //Assert
                Assert.True(result);
            }

            private static DefaultExpression BuildSubject()
            {
                var symbol = Mock.Of<Default>();
                return new DefaultExpression(symbol);
            }
        }

        new public class ToString
        {
            [Fact]
            public void returns_string_from_symbol()
            {
                //Arrange
                var symbol = new Mock<Default>();
                symbol.Setup(s => s.ToString()).Returns("test");
                var subject = new DefaultExpression(symbol.Object);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
