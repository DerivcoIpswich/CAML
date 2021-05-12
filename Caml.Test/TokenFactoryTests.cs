using Caml.Ast;
using Caml.Parser;
using System;
using Xunit;

using static Caml.Ast.TokenKind;

namespace Caml
{
    public class TokenFactoryTests
    {
        public class BuildEnd
        {
            [Fact]
            public void returns_end_token()
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.BuildEnd();

                //Assert
                Assert.Equal(TokenKind.End, result.Kind);
            }
        }

        public class IsReserved
        {
            [Theory]
            [InlineData('$')]
            public void returns_true_for_reserved_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsReserved(character);

                //Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData('a')]
            [InlineData('1')]
            [InlineData('£')]
            public void returns_false_for_non_reserved_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsReserved(character);

                //Assert
                Assert.False(result);
            }
        }

        public class IsTrivia
        {
            [Theory]
            [InlineData(' ')]
            [InlineData('\t')]
            public void returns_true_for_trivia_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsTrivia(character);

                //Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData('a')]
            [InlineData('1')]
            [InlineData('£')]
            [InlineData('!')]
            public void returns_false_for_non_reserved_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsTrivia(character);

                //Assert
                Assert.False(result);
            }
        }

        public class IsBoundry
        {
            [Theory]
            [InlineData('&')]
            [InlineData('|')]
            [InlineData('^')]
            [InlineData('(')]
            [InlineData(41)] //HACK ')' causes an error in xunit for vs
            [InlineData(',')]
            [InlineData('!')]
            public void returns_true_for_boundry_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsBoundry(character);

                //Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData(' ')]
            [InlineData('\t')]
            public void returns_true_for_whitespace_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsBoundry(character);

                //Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData('a')]
            [InlineData('1')]
            [InlineData('£')]
            public void returns_false_for_non_boundry_characters(char character)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.IsBoundry(character);

                //Assert
                Assert.False(result);
            }
        }

        public class BuildTokens
        {
            [Fact]
            public void throws_when_passed_null()
            {
                var subject = new TokenFactory();

                //Act
                Action action = () => subject.BuildToken(null);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Fact]
            public void throws_when_passed_whitespace_string()
            {
                var subject = new TokenFactory();

                //Act
                Action action = () => subject.BuildToken("");
                Action action2 = () => subject.BuildToken(" ");
                Action action3 = () => subject.BuildToken("\t");

                //Assert
                Assert.Throws<ArgumentException>(action);
                Assert.Throws<ArgumentException>(action2);
                Assert.Throws<ArgumentException>(action3);
            }

            [Theory]
            [InlineData("default", Default)]
            [InlineData("&", And)]
            [InlineData("|", Or)]
            [InlineData("^", XOr)]
            [InlineData("!", Not)]
            [InlineData("(", OpenParenthesis)]
            [InlineData(")", CloseParenthesis)]
            [InlineData(",", Comma)]
            public void builds_predefined_token_from_string_value(string input, TokenKind expectedToken)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.BuildToken(input);

                //Assert
                Assert.Equal(expectedToken, result.Kind);
            }

            [Theory]
            [InlineData(Default, "default")]
            [InlineData(And, "&")]
            [InlineData(Or , "|")]
            [InlineData(XOr, "^")]
            [InlineData(Not, "!")]
            [InlineData(OpenParenthesis, "(")]
            [InlineData(CloseParenthesis, ")")]
            [InlineData(Comma, ",")]
            public void builds_predefined_token_from_tokenkind(TokenKind tokenKind, string expectedValue)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.BuildToken(tokenKind);

                //Assert
                Assert.Equal(tokenKind, result.Kind);
                Assert.Equal(expectedValue, result.Value);
            }

            [Theory]
            [InlineData("test")]
            [InlineData("123")]
            [InlineData("false")]
            [InlineData("false123")]
            public void builds_literal_token(string input)
            {
                //Arrange
                var subject = new TokenFactory();

                //Act
                var result = subject.BuildToken(input);

                //Assert
                Assert.Equal(Literal, result.Kind);
            }

            [Fact]
            public void throws_when_passed_literal_tokenkind()
            {
                var subject = new TokenFactory();

                //Act
                Action action = () => subject.BuildToken(TokenKind.Literal);

                //Assert
                Assert.Throws<ArgumentException>(action);
            }

            [Fact]
            public void throws_when_passed_invalid_tokenkind()
            {
                var subject = new TokenFactory();

                //Act
                Action action = () => subject.BuildToken((TokenKind)(-1));

                //Assert
                Assert.Throws<ArgumentException>(action);
            }
        }
    }
}
