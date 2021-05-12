using Caml.Ast.Symbols;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Caml.Ast
{
    public class FunctionTests
    {
        new public class ToString
        {
            [Fact]
            public void returns_function_without_arguments()
            {
                //Arrange
                Function subject = BuildSubject();

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test[]", result);
            }

            [Fact]
            public void returns_function_with_single_argument()
            {
                //Arrange
                Function subject = BuildSubject("a");

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test[a]", result);
            }

            [Fact]
            public void returns_function_with_two_arguments()
            {
                //Arrange
                Function subject = BuildSubject("a", "b");

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test[a, b]", result);
            }

            [Fact]
            public void returns_function_with_three_arguments()
            {
                //Arrange
                Function subject = BuildSubject("a", "b", "c");

                //Act
                var result = subject.ToString();

                //Assert
                Assert.Equal("test[a, b, c]", result);
            }

            private static Function BuildSubject(params string[] args)
            {
                return new Function
                {
                    Open = new Token("[", TokenKind.OpenParenthesis),
                    Name = new Token("test", TokenKind.Default),
                    Close = new Token("]", TokenKind.CloseParenthesis),
                    Arguments = args.Select(BuildArgument).ToList()
                };
            }

            private static Literal BuildArgument(string name)
            {
                var mock = new Mock<Literal>();
                mock.Setup(m => m.ToString()).Returns(name);
                return mock.Object;
            }
        }
    }
}
