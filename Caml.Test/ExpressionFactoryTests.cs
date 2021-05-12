using Caml.Ast;
using Caml.Ast.Symbols;
using Caml.Expressions;
using Moq;
using System;
using Xunit;

namespace Caml
{
    public class ExpressionFactoryTests
    {
        public class BuildExpression
        {
            [Fact]
            public void throws_when_passed_null_symbol()
            {
                //Arrange
                var subject = new ExpressionFactory();

                //Act
                Action action = () => subject.BuildExpression(null);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Fact]
            public void throws_when_passed_invalid_symbol()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var mockSymbol = new Mock<ISymbol>();
                mockSymbol.Setup(s => s.Kind).Returns((SymbolKind)(-1));

                //Act
                Action action = () => subject.BuildExpression(mockSymbol.Object);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void returns_DefaultExpression_when_passed_symbol_with_SymbolKind_Default()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = Mock.Of<Default>();

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<DefaultExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Fact]
            public void returns_LiteralExpression_when_passed_symbol_with_SymbolKind_Literal()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = Mock.Of<Literal>();

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<LiteralExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Fact]
            public void returns_AndExpression_when_passed_symbol_with_operator_with_TokenKind_And()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockBinaryOperation(TokenKind.And);

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<AndExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Fact]
            public void returns_OrExpression_when_passed_symbol_with_operator_with_TokenKind_Or()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockBinaryOperation(TokenKind.Or);

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<OrExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Fact]
            public void returns_ExclusiveOrExpression_when_passed_symbol_with_operator_with_TokenKind_XOr()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockBinaryOperation(TokenKind.XOr);

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<ExclusiveOrExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Theory]
            [InlineData(TokenKind.CloseParenthesis)]
            [InlineData(TokenKind.Comma)]
            [InlineData(TokenKind.Default)]
            [InlineData(TokenKind.End)]
            [InlineData(TokenKind.Literal)]
            [InlineData(TokenKind.Not)]
            [InlineData(TokenKind.OpenParenthesis)]
            public void throws_when_passed_BinaryOperation_with_invalid_TokenKind(TokenKind tokenKind)
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockBinaryOperation(tokenKind);

                //Act
                Action action = () => subject.BuildExpression(symbol);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void returns_NotExpression_when_passed_symbol_with_operator_with_TokenKind_Not()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockUnaryPrefixOperation(TokenKind.Not);

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<NotExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Theory]
            [InlineData(TokenKind.CloseParenthesis)]
            [InlineData(TokenKind.Comma)]
            [InlineData(TokenKind.Default)]
            [InlineData(TokenKind.End)]
            [InlineData(TokenKind.Literal)]
            [InlineData(TokenKind.OpenParenthesis)]
            [InlineData(TokenKind.And)]
            [InlineData(TokenKind.Or)]
            [InlineData(TokenKind.XOr)]
            public void throws_when_passed_UnaryPrefixOperation_with_invalid_TokenKind(TokenKind tokenKind)
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockUnaryPrefixOperation(tokenKind);

                //Act
                Action action = () => subject.BuildExpression(symbol);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void returns_FunctionExpression_when_passed_symbol_with_SymbolKind_Function()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = Mock.Of<Function>();

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<FunctionExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            [Fact]
            public void returns_GroupedExpression_when_passed_symbol_with_SymbolKind_ExpressionGroup()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var symbol = MockExpressionGroup();

                //Act
                var result = subject.BuildExpression(symbol);

                //Assert
                Assert.IsType<GroupedExpression>(result);
                Assert.Same(symbol, result.Symbol);
            }

            private static ISymbol MockBinaryOperation(TokenKind tokenKind)
            {
                var symbol = new Mock<BinaryOperation>();
                symbol.Setup(s => s.Left).Returns(Mock.Of<Literal>());
                symbol.Setup(s => s.Operator).Returns(new Token("test", tokenKind));
                symbol.Setup(s => s.Right).Returns(Mock.Of<Literal>());
                return symbol.Object;
            }

            private static ISymbol MockUnaryPrefixOperation(TokenKind tokenKind)
            {
                var symbol = new Mock<UnaryPrefixOperation>();
                symbol.Setup(s => s.Operator).Returns(new Token("test", tokenKind));
                symbol.Setup(s => s.Expression).Returns(Mock.Of<Literal>());
                return symbol.Object;
            }

            private static ISymbol MockExpressionGroup()
            {
                var symbol = new Mock<ExpressionGroup>();
                symbol.Setup(s => s.Expression).Returns(Mock.Of<Literal>());
                return symbol.Object;
            }
        }

        public class BuildBinaryExpression
        {
            [Fact]
            public void returns_AndExpression_when_passed_BinaryOperation_with_TokenKind_And()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var left = Mock.Of<IExpression>();
                var right = Mock.Of<IExpression>();
                var token = new Token("and", TokenKind.And);

                //Act
                var result = subject.BuildBinaryExpression(left, token, right);

                //Assert
                Assert.IsType<AndExpression>(result);
                Assert.Same(left, result.Left);
                Assert.Same(right, result.Right);
                Assert.Equal(SymbolKind.BinaryOperation, result.Symbol.Kind);
            }

            [Fact]
            public void returns_OrExpression_when_passed_BinaryOperation_with_TokenKind_Or()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var left = Mock.Of<IExpression>();
                var right = Mock.Of<IExpression>();
                var token = new Token("or", TokenKind.Or);

                //Act
                var result = subject.BuildBinaryExpression(left, token, right);

                //Assert
                Assert.IsType<OrExpression>(result);
                Assert.Same(left, result.Left);
                Assert.Same(right, result.Right);
                Assert.Equal(SymbolKind.BinaryOperation, result.Symbol.Kind);
            }

            [Fact]
            public void returns_ExclusiveOrExpression_when_passed_BinaryOperation_with_TokenKind_XOr()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var left = Mock.Of<IExpression>();
                var right = Mock.Of<IExpression>();
                var token = new Token("xor", TokenKind.XOr);

                //Act
                var result = subject.BuildBinaryExpression(left, token, right);

                //Assert
                Assert.IsType<ExclusiveOrExpression>(result);
                Assert.Same(left, result.Left);
                Assert.Same(right, result.Right);
                Assert.Equal(SymbolKind.BinaryOperation, result.Symbol.Kind);
            }

            [Theory]
            [InlineData(TokenKind.Comma)]
            [InlineData(TokenKind.Default)]
            [InlineData(TokenKind.End)]
            [InlineData(TokenKind.Literal)]
            [InlineData(TokenKind.Not)]
            [InlineData(TokenKind.OpenParenthesis)]
            public void throws_when_passed_invalid_TokenKind(TokenKind tokenKind)
            {
                //Arrange
                var subject = new ExpressionFactory();
                var left = Mock.Of<IExpression>();
                var right = Mock.Of<IExpression>();
                var token = new Token("invalid", tokenKind);

                //Act
                Action action = () => subject.BuildBinaryExpression(left, token, right);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void throws_when_passed_null_left_expression()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var right = Mock.Of<IExpression>();
                var token = new Token("and", TokenKind.And);

                //Act
                Action action = () => subject.BuildBinaryExpression(null, token, right);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Fact]
            public void throws_when_passed_null_right_expression()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var left = Mock.Of<IExpression>();
                var token = new Token("and", TokenKind.And);

                //Act
                Action action = () => subject.BuildBinaryExpression(left, token, null);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }
        }

        public class BuildUnaryPrefixExpression
        {
            [Fact]
            public void returns_NotExpression_when_passed_UnaryPrefixOperation_with_TokenKind_Not()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var expression = Mock.Of<IExpression>();
                var token = new Token("not", TokenKind.Not);

                //Act
                var result = subject.BuildUnaryPrefixExpression(token, expression);

                //Assert
                Assert.IsType<NotExpression>(result);
                Assert.Same(expression, result.Expression);
                Assert.Equal(SymbolKind.UnaryPrefixOperation, result.Symbol.Kind);
            }

            [Fact]
            public void throws_when_passed_null_expression()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var token = new Token("not", TokenKind.Not);

                //Act
                Action action = () => subject.BuildUnaryPrefixExpression(token, null);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Theory]
            [InlineData(TokenKind.CloseParenthesis)]
            [InlineData(TokenKind.Comma)]
            [InlineData(TokenKind.Default)]
            [InlineData(TokenKind.End)]
            [InlineData(TokenKind.Literal)]
            [InlineData(TokenKind.OpenParenthesis)]
            [InlineData(TokenKind.And)]
            [InlineData(TokenKind.Or)]
            [InlineData(TokenKind.XOr)]
            public void throws_when_passed_invalid_TokenKind(TokenKind tokenKind)
            {
                //Arrange
                var subject = new ExpressionFactory();
                var expression = Mock.Of<IExpression>();
                var token = new Token("invalid", tokenKind);

                //Act
                Action action = () => subject.BuildUnaryPrefixExpression(token, expression);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }
        }

        public class BuildExpressionGroup
        {
            [Fact]
            public void returns_GroupedExpression_when_passed_expression()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var expression = Mock.Of<IExpression>();
                var open = new Token("open", TokenKind.OpenParenthesis);
                var close = new Token("close", TokenKind.CloseParenthesis);

                //Act
                var result = subject.BuildGroupedExpression(open, expression, close);

                //Assert
                Assert.IsType<GroupedExpression>(result);
                Assert.Same(expression, result.Expression);
                Assert.Equal(SymbolKind.ExpressionGroup, result.Symbol.Kind);
            }

            [Fact]
            public void throws_when_passed_null_expression()
            {
                //Arrange
                var subject = new ExpressionFactory();
                var open = new Token("open", TokenKind.OpenParenthesis);
                var close = new Token("close", TokenKind.CloseParenthesis);

                //Act
                Action action = () => subject.BuildGroupedExpression(open, null, close);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }
        }
    }
}
