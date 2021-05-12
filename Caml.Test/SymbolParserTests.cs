using Caml.Ast;
using Caml.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Caml.Ast.TokenKind;

namespace Caml
{
    public class SymbolParserTests
    {
        public class Parse
        {
            [Fact]
            public void throws_when_passed_null()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(null);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Fact]
            public void throws_when_passed_empty_tokens()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(Enumerable.Empty<Token>());

                //Assert
                Assert.Throws<ArgumentException>(action);
            }

            [Fact]
            public void throws_when_passed_end()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(End));

                //Assert
                Assert.Throws<ArgumentException>(action);
            }

            [Theory]
            [InlineData(CloseParenthesis)]
            [InlineData(And)]
            [InlineData(Or)]
            [InlineData(Comma)]
            public void throws_when_passed_unexpected_token(TokenKind token)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(token));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void parses_default()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(Default, End));

                //Assert
                Assert.Equal(SymbolKind.Default, result.Kind);
            }

            [Theory]
            [InlineData(Default)]
            [InlineData(Default, Default, End)]
            [InlineData(Default, Literal, End)]
            [InlineData(Default, And, End)]
            [InlineData(Default, Or, End)]
            [InlineData(Default, Not, End)]
            [InlineData(Default, OpenParenthesis, End)]
            [InlineData(Default, CloseParenthesis, End)]
            [InlineData(Default, Comma, End)]
            public void throws_when_default_not_followed_by_end(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void parses_literal()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(Literal, End));

                //Assert
                Assert.Equal(SymbolKind.Literal, result.Kind);
            }

            [Theory]
            [InlineData(Literal, Default, End)]
            [InlineData(Literal, Literal, End)]
            [InlineData(Literal, Not, End)]
            [InlineData(Default, CloseParenthesis, End)]
            [InlineData(Default, Comma, End)]
            public void throws_when_literal_followed_by_an_unexpected_token(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Fact]
            public void parses_function_with_no_arguments()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(Literal, OpenParenthesis, CloseParenthesis, End));

                //Assert
                Assert.Equal(SymbolKind.Function, result.Kind);
                var symbol = result as Ast.Symbols.Function;
                Assert.Empty(symbol.Arguments);
            }

            [Fact]
            public void parses_function_with_single_argument()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(Literal, OpenParenthesis, Literal, CloseParenthesis, End));

                //Assert
                Assert.Equal(SymbolKind.Function, result.Kind);
                var symbol = result as Ast.Symbols.Function;
                Assert.Single(symbol.Arguments);
            }

            [Theory]
            [InlineData(2)]
            [InlineData(3)]
            [InlineData(4)]
            [InlineData(5)]
            public void parses_function_with_multiple_arguments(int arity)
            {
                //Arrange
                var subject = new SymbolParser();

                var arguments = Enumerable.Repeat(MockTokens(Literal, Comma), arity)
                    .SelectMany(a => a)
                    .SkipLast(1);

                var tokens = MockTokens(Literal, OpenParenthesis)
                    .Concat(arguments)
                    .Concat(MockTokens(CloseParenthesis, End));

                //Act
                var result = subject.Parse(tokens);

                //Assert
                Assert.Equal(SymbolKind.Function, result.Kind);
                var symbol = result as Ast.Symbols.Function;
                Assert.Equal(arity, symbol.Arguments.Count());
            }

            [Fact]
            public void throws_when_single_function_argument_is_followed_by_comma()
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(Literal, OpenParenthesis, Literal, Comma, CloseParenthesis, End));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Theory]
            [InlineData(2)]
            [InlineData(3)]
            [InlineData(4)]
            [InlineData(5)]
            public void throws_when_function_arguments_are_not_deliminated_by_comma(int arity)
            {
                //Arrange
                var subject = new SymbolParser();
                var tokens = MockTokens(Literal, OpenParenthesis)
                    .Concat(Enumerable.Repeat(MockToken(Literal), arity))
                    .Concat(MockTokens(CloseParenthesis, End));

                //Act
                Action action = () => subject.Parse(tokens);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Theory]
            [InlineData(Default)]
            [InlineData(End)]
            [InlineData(And)]
            [InlineData(Or)]
            [InlineData(Not)]
            [InlineData(OpenParenthesis)]
            [InlineData(CloseParenthesis)]
            [InlineData(Literal, Comma, End)]
            [InlineData(Literal, Comma, Comma)]
            [InlineData(Comma)]
            public void throws_when_function_arguments_are_not_literals(params TokenKind[] arguments)
            {
                //Arrange
                var subject = new SymbolParser();
                var tokens = MockTokens(Literal, OpenParenthesis)
                    .Concat(MockTokens(arguments))
                    .Concat(MockTokens(CloseParenthesis, End));

                //Act
                Action action = () => subject.Parse(tokens);

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Theory]
            [InlineData(Default, End)]
            [InlineData(Literal, End)]
            [InlineData(Not, End)]
            [InlineData(OpenParenthesis, End)]
            [InlineData(CloseParenthesis, End)]
            [InlineData(Comma, End)]
            public void throws_when_function_is_followed_by_an_unexpected_token(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(Literal, OpenParenthesis, CloseParenthesis).Concat(MockTokens(tokens)));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Theory]
            [InlineData(Not, Literal, End)]
            [InlineData(Not, Literal, OpenParenthesis, CloseParenthesis, End)]
            [InlineData(Not, OpenParenthesis, Literal, CloseParenthesis, End)]
            [InlineData(Not, Not, Literal, End)]
            public void parses_unary_prefix_operation(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Equal(SymbolKind.UnaryPrefixOperation, result.Kind);
                var symbol = result as Ast.Symbols.UnaryPrefixOperation;
                Assert.Equal(tokens[0], symbol.Operator.Kind);
            }

            [Theory]
            [InlineData(Not, End)]
            [InlineData(Not, Not, End)]
            [InlineData(Not, Default, End)]
            [InlineData(Not, And, End)]
            [InlineData(Not, Or, End)]
            [InlineData(Not, XOr, End)]
            [InlineData(Not, CloseParenthesis, End)]
            [InlineData(Not, Comma, End)]
            public void throws_when_unary_prefix_operation_is_followed_by_an_unexpected_token(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Theory]
            [InlineData(Or, Literal, Or, Literal, End)]
            [InlineData(XOr, Literal, XOr, Literal, End)]
            [InlineData(Or, OpenParenthesis, Literal, CloseParenthesis, Or, Literal, End)]
            [InlineData(Or, Literal, Or, OpenParenthesis, Literal, CloseParenthesis, End)]
            [InlineData(Or, Literal, Or, Literal, OpenParenthesis, CloseParenthesis, End)]
            [InlineData(Or, Not, Literal, Or, Literal, End)]
            [InlineData(And, Literal, And, Not, Literal, End)]
            [InlineData(Or, Literal, Or, Literal, And, Literal, End)]
            [InlineData(And, Literal, And, Literal, Or, Literal, End)]
            [InlineData(Or, Literal, OpenParenthesis, CloseParenthesis, Or, Literal, End)]
            [InlineData(Or, Not, Literal, OpenParenthesis, CloseParenthesis, Or, Literal, End)]
            public void parses_binary_operation(TokenKind operatorKind, params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Equal(SymbolKind.BinaryOperation, result.Kind);
                var symbol = result as Ast.Symbols.BinaryOperation;
                Assert.Equal(operatorKind, symbol.Operator.Kind);
            }

            [Theory]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, End)]
            [InlineData(OpenParenthesis, Literal, OpenParenthesis, CloseParenthesis, CloseParenthesis, End)]
            [InlineData(OpenParenthesis, Literal, And, Literal, CloseParenthesis, End)]
            public void parses_expression_group(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                var result = subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Equal(SymbolKind.ExpressionGroup, result.Kind);
            }

            [Theory]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, Default, End)]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, Literal, End)]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, Not, End)]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, OpenParenthesis, End)]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, CloseParenthesis, End)]
            [InlineData(OpenParenthesis, Literal, CloseParenthesis, Comma, End)]
            public void throws_when_expression_group_is_followed_by_an_unexpected_token(params TokenKind[] tokens)
            {
                //Arrange
                var subject = new SymbolParser();

                //Act
                Action action = () => subject.Parse(MockTokens(tokens));

                //Assert
                Assert.Throws<InvalidOperationException>(action);
            }
        }

        private static IEnumerable<Token> MockTokens(params TokenKind[] list) => list.Select(MockToken);

        private static Token MockToken(TokenKind kind) => new Token(kind.ToString(), kind);
    }
}
