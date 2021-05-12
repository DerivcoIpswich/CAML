using System;
using Xunit;

namespace Caml
{
    public class ConfigurableMatcherTests
    {
        public class a_new_ConfigurableMatcher
        {
            [Fact]
            public void contains_no_functions()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                var result = subject.Functions;

                //Assert
                Assert.Empty(result);
            }

            [Fact]
            public void contains_no_literals()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                var result = subject.Functions;

                //Assert
                Assert.Empty(result);
            }
        }

        public class AddMatch
        {
            [Fact]
            public void adds_Literal()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                subject.AddMatch("test");

                //Assert
                Assert.Equal(new[] { "test" }, subject.Literals);
            }

            [Fact]
            public void adds_SingleValueMatcher()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                subject.AddMatch("test", (string _) => true);

                //Assert
                Assert.Equal(new[] { "test" }, subject.Functions);
            }

            [Fact]
            public void adds_MultiValueMatcher()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                subject.AddMatch("test", (string[] _) => true);

                //Assert
                Assert.Equal(new[] { "test" }, subject.Functions);
            }

            [Fact]
            public void does_not_add_duplicate_literal()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                subject.AddMatch("test");

                //Act
                subject.AddMatch("test");

                //Assert
                Assert.Equal(new[] { "test" }, subject.Literals);
            }

            [Fact]
            public void throws_when_adding_duplicate_SingleValueMatcher()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                subject.AddMatch("test", (string _) => true);

                //Act
                Action action = () => subject.AddMatch("test", (string _) => false);

                //Assert
                Assert.Throws<ArgumentException>(action);
            }

            [Fact]
            public void throws_when_adding_duplicate_MultiValueMatcher()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                subject.AddMatch("test", (string[] _) => true);

                //Act
                Action action = () => subject.AddMatch("test", (string[] _) => false);

                //Assert
                Assert.Throws<ArgumentException>(action);
            }

            [Fact]
            public void throws_when_adding_duplicate_function()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                subject.AddMatch("test", (string _) => true);

                //Act
                Action action = () => subject.AddMatch("test", (string[] _) => false);

                //Assert
                Assert.Throws<ArgumentException>(action);
            }
        }

        public class Match
        {
            [Fact]
            public void returns_false_when_literal_is_not_defined_and_no_arguments_supplied()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                var result = subject.Match("test");

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void returns_true_when_literal_is_defined_and_no_arguments_supplied()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                subject.AddMatch("test");

                //Act
                var result = subject.Match("test");

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void delegates_to_SingleValueMatcher_when_single_argument_is_supplied()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                var wasCalled = false;
                subject.AddMatch("test", (string _) => wasCalled = true);

                //Act
                subject.Match("test", "arg");

                //Assert
                Assert.True(wasCalled);
            }

            [Fact]
            public void delegates_to_MultiValueMatcher_when_multiple_arguments_are_supplied()
            {
                //Arrange
                var subject = new ConfigurableMatcher();
                var wasCalled = false;
                subject.AddMatch("test", (string[] _) => wasCalled = true);

                //Act
                subject.Match("test", "arg1", "arg2", "arg3");

                //Assert
                Assert.True(wasCalled);
            }

            [Fact]
            public void throws_when_no_matching_function_is_defined_and_arguments_are_supplied()
            {
                //Arrange
                var subject = new ConfigurableMatcher();

                //Act
                Action action1 = () => subject.Match("test", "arg");
                Action action2 = () => subject.Match("test", "arg", "arg2", "arg3");

                //Assert
                Assert.Throws<ArgumentException>(action1);
                Assert.Throws<ArgumentException>(action2);
            }
        }
    }
}
