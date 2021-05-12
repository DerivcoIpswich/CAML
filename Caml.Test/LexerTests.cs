using Caml.Ast;
using Caml.Parser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Caml
{
    public class LexerTests
    {
        public class Constructor
        {
            [Fact]
            public void throws_when_arguments_are_null()
            {
                //Arrange

                //Act
                Action action = () => new Lexer(null);

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }
        }

        public class Tokenise
        {
            [Fact]
            public void is_lazy_evaluated()
            {
                //Arrange
                var mockString = new Mock<IEnumerable<char>>();
                var mockEnumerator = new Mock<IEnumerator<char>>();
                mockEnumerator.Setup(e => e.MoveNext()).Returns(false);
                mockString.Setup(s => s.GetEnumerator()).Returns(mockEnumerator.Object);
                var subject = new Lexer(BuildTokenFactory());

                //Act
                var result = subject.Tokenise(mockString.Object);

                //Assert
                mockEnumerator.Verify(e => e.MoveNext(), Times.Never);
                result.ToArray();
                mockEnumerator.Verify(e => e.MoveNext(), Times.Once);
            }

            [Fact]
            public void throws_when_passed_null()
            {
                //Arrange
                var subject = new Lexer(BuildTokenFactory());

                //Act
                Action action = () => subject.Tokenise(null).Any();

                //Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Fact]
            public void returns_end_token_when_passed_empty_string()
            {
                //Arrange
                var endToken = new Token("end", TokenKind.End);
                var subject = new Lexer(BuildTokenFactory(endToken: endToken));

                //Act
                var result = subject.Tokenise(string.Empty);

                //Assert          
                Assert.Single(result);
                Assert.Equal(endToken, result.Single());
            }

            [Fact]
            public void throws_for_reserved_characters()
            {
                //Arrange
                var subject = new Lexer(BuildTokenFactory(reservedChars: "$!"));

                //Act
                Action action1 = () => subject.Tokenise("$").Any();
                Action action2 = () => subject.Tokenise("!").Any();

                //Assert
                Assert.Throws<ArgumentException>(action1);
                Assert.Throws<ArgumentException>(action2);
            }

            [Fact]
            public void enumerates_a_token_when_no_boundry_characters_are_present()
            {
                //Arrange
                var token = new Token("test-is-a-test", TokenKind.Literal);
                var subject = new Lexer(BuildTokenFactory(boundryChars: " ", tokens: new[] { token }));

                //Act
                var result = subject.Tokenise("test-is-a-test");

                //Assert
                Assert.Equal(token, result.First());
                Assert.Equal(2, result.Count());
            }

            [Fact]
            public void enumerates_a_token_before_a_boundry_character()
            {
                //Arrange
                var token = new Token("test", TokenKind.Literal);
                var subject = new Lexer(BuildTokenFactory(boundryChars: " ", tokens: new[] { token }));

                //Act
                var result = subject.Tokenise("test is a test");

                //Assert
                Assert.Equal(token, result.First());
            }

            [Fact]
            public void enumerates_a_token_for_each_boundry_character()
            {
                //Arrange
                var boundryToken = new Token("-", TokenKind.Literal);
                var endToken = new Token("end", TokenKind.End);
                var subject = new Lexer(BuildTokenFactory(boundryChars: "-", endToken:endToken, tokens: new[] { boundryToken, boundryToken, boundryToken }));

                //Act
                var result = subject.Tokenise("---");

                //Assert
                Assert.Equal(new[] { boundryToken, boundryToken, boundryToken, endToken }, result);
            }

            [Fact]
            public void does_not_enumerate_a_token_for_a_whitespace_character()
            {
                //Arrange
                var tokenFactory = BuildTokenFactory(triviaChars: "123");
                var subject = new Lexer(tokenFactory);

                //Act
                subject.Tokenise("121123").Any();

                //Assert
                Mock.Get(tokenFactory).Verify(t => t.BuildToken(It.IsAny<string>()), Times.Never);
            }

            [Fact]
            public void enumerates_a_sequence_of_tokens_split_by_boundry_characters()
            {
                //Arrange
                var token1 = new Token("1", TokenKind.Literal);
                var token2 = new Token("1", TokenKind.Literal);
                var token3 = new Token("2", TokenKind.Literal);
                var boundryToken = new Token("-", TokenKind.Literal);
                var endToken = new Token("end", TokenKind.End);
                var subject = new Lexer(BuildTokenFactory(boundryChars: "-", endToken: endToken, tokens: new[] { token1, boundryToken, token2, boundryToken, token3 }));

                //Act
                var result = subject.Tokenise("1-2-3");

                //Assert
                Assert.Equal(new[] { token1, boundryToken, token2, boundryToken, token3, endToken }, result);
            }

            private static ITokenFactory BuildTokenFactory(
                IEnumerable<char> reservedChars = default,
                IEnumerable<char> boundryChars = default, 
                IEnumerable<char> triviaChars = default, 
                Token endToken = default, 
                Token[] tokens = default)
            {
                var mock = new Mock<ITokenFactory>();
                foreach (var reserved in reservedChars ?? Enumerable.Empty<char>())
                {
                    mock.Setup(e => e.IsReserved(reserved)).Returns(true);
                }

                foreach (var boundry in boundryChars ?? Enumerable.Empty<char>())
                {
                    mock.Setup(e => e.IsBoundry(boundry)).Returns(true);
                }

                foreach (var trivia in triviaChars ?? Enumerable.Empty<char>())
                {
                    mock.Setup(e => e.IsTrivia(trivia)).Returns(true);
                }

                mock.Setup(t => t.BuildEnd()).Returns(() => endToken);

                var sequence = new MockSequence();
                foreach (var token in tokens ?? Enumerable.Empty<Token>())
                {
                    mock.InSequence(sequence).Setup(m => m.BuildToken(It.IsAny<string>())).Returns(token);
                }

                return mock.Object;
            }

        }
    }
}
