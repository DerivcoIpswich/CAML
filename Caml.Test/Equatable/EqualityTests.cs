using Caml.Expressions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Caml.Test.Equatable
{
    public class EqualityTests
    {
        new public class Equals
        {
            [Theory]
            [MemberData(nameof(GetEquivalentTestData))]
            public void returns_true_for_equivalent_expressions(IExpression a, IExpression b)
            {
                var result = a.Equals(b);
                Assert.True(result);
            }

            [Theory]
            [MemberData(nameof(GetNonEquivalentTestData))]
            public void returns_false_for_non_equivalent_expressions(IExpression a, IExpression b)
            {
                var result = a.Equals(b);
                Assert.False(result);
            }

            public static IEnumerable<object[]> GetEquivalentTestData()
            {
                Func<string, IExpression> build = new ExpressionFactory().BuildExpression;
                object[] Compare(string a, string b)
                {
                    return new[] { build(a), build(b) };
                }

                //identity
                yield return Compare("a", "a");
                yield return Compare("func(a)", "func(a)");
                yield return Compare("!a", "!a");
                yield return Compare("(a)", "(a)");
                yield return Compare("a & b", "a & b");
                yield return Compare("a | b", "a | b");
                yield return Compare("a ^ b", "a ^ b");
                yield return Compare("default", "default");

                //tautology
                yield return Compare("a & a", "a");
                yield return Compare("a | a", "a");
                
                //double negation
                yield return Compare("!!a", "a");
                yield return Compare("!(!a)", "a");
                yield return Compare("a ^ a ^ a", "a");

                //commutative
                yield return Compare("a & b", "b & a");
                yield return Compare("a | b", "b | a");
                yield return Compare("a ^ b", "b ^ a");
                yield return Compare("func(a) & b", "b & func(a)");

                //associative
                yield return Compare("(a | b) | c", "a | (b | c)");
                yield return Compare("(a & b) & c", "a & (b & c)");
                yield return Compare("(a ^ b) ^ c", "a ^ (b ^ c)");
                
                //distributive
                yield return Compare("a | (b & c)", "(a | b) & (a | c)");
                yield return Compare("a & (b | c)", "(a & b) | (a & c)");
                
                //De Morgan's
                yield return Compare("!(a & b)", "!a | !b");
                yield return Compare("!(a | b)", "!a & !b");
                
                //absorption
                yield return Compare("a | (a & b)", "a");
                yield return Compare("a & (a | b)", "a");
                yield return Compare("func(a) & func(a)", "func(a)");

                //functions
                yield return Compare("func1(a, b, c) & func2(a)", "func2(a) & func1(a, b, c)");
            }

            public static IEnumerable<object[]> GetNonEquivalentTestData()
            {
                Func<string, IExpression> build = new ExpressionFactory().BuildExpression;
                object[] Compare(string a, string b)
                {
                    return new[] { build(a), build(b) };
                }

                yield return Compare("a & a", "a | b");
                yield return Compare("a & a", "a ^ b");
                yield return Compare("a | a", "a & b");
                yield return Compare("a | a", "a ^ b");

                //self-inverse
                yield return Compare("!a", "a");
                yield return Compare("a ^ a", "a");
                
                //literals
                yield return Compare("a", "b");
                yield return Compare("a", "a & b");
                
                //functions
                yield return Compare("func(a)", "func(b)");
                yield return Compare("func(a, b)", "func(a)");
                
                //default
                yield return Compare("default", "a");
                yield return Compare("a", "default");
            }
        }
    }
}
