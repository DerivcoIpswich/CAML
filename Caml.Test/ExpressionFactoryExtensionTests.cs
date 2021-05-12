using Caml.Ast;
using Caml.Expressions;
using Xunit;

namespace Caml
{
    public class ExpressionFactoryExtensionTests
    {
        public class BuildDefaultExpression
        {
            [Fact]
            public void returns_DefaultExpression()
            {
                //Arrange
                var subject = new ExpressionFactory();

                //Act
                var result = subject.BuildDefaultExpression();

                //Assert
                Assert.IsType<DefaultExpression>(result);
                Assert.Equal(SymbolKind.Default, result.Symbol.Kind);
            }
        }
    }
}