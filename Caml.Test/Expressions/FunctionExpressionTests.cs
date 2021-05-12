using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Caml.Expressions
{
    public class FunctionExpressionTests
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
                matcher.Setup(m => m.Match("fn", "arg")).Returns(matcherReturns).Verifiable();
                var symbol = new Mock<Function>();
                var arg = new Mock<Literal>();
                arg.Setup(a => a.Token).Returns(new Token("arg", TokenKind.Literal));
                symbol.Setup(s => s.Arguments).Returns(new List<Literal> { arg.Object });
                symbol.Setup(s => s.Name).Returns(new Token("fn", TokenKind.Literal));

                var subject = new FunctionExpression(symbol.Object);

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
                var symbol = new Mock<Function>();
                symbol.Setup(s => s.ToString()).Returns("test");
                var subject = new FunctionExpression(symbol.Object);

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test", result);
            }
        }
    }
}
