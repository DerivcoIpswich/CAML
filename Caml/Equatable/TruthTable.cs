using Caml.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Caml.Equatable
{
    internal class TruthTable : IEquatable<TruthTable>
    {
        private readonly IExpression expression;

        private IEnumerable<string> Terms { get; }

        private static IEnumerable<IDictionary<string, bool>> GetInputs(IEnumerable<string> terms)
        {
            return terms.GetPermutations().Select(p => terms.ToDictionary(t => t, p.Contains));
        }

        public TruthTable(IExpression expression)
        {
            this.expression = expression;
            Terms = new TermsVisitor().GetTerms(expression);
        }

        public bool Equals(TruthTable other)
        {
            return Equals(this, other);
        }

        public static bool Equals(TruthTable a, TruthTable b)
        {
            var allTerms = a.Terms.Concat(b.Terms).Distinct();
            var inputs = GetInputs(allTerms);
            var matchers = inputs.Select(i => new TermsMatcher(i));
            var aOutput = matchers.Select(m => a.expression.IsMatch(m));
            var bOutput = matchers.Select(m => b.expression.IsMatch(m));
            return aOutput.SequenceEqual(bOutput);
        }

        public override string ToString()
        {
            return expression.ToString();
        }
    }
}
