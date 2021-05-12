using System;
using System.Collections.Generic;
using System.Linq;

namespace Caml
{
    public class ConfigurableMatcher : IMatcher
    {
        public delegate bool MultiValueMatcher(string[] values);
        public delegate bool SingleValueMatcher(string value);

        private readonly Dictionary<string, MultiValueMatcher> functions;
        private readonly HashSet<string> literals;

        public IEnumerable<string> Functions => functions.Keys;

        public IEnumerable<string> Literals => literals;

        public ConfigurableMatcher()
        {
            functions = new Dictionary<string, MultiValueMatcher>();
            literals = new HashSet<string>();
        }

        public void AddMatch(string name, MultiValueMatcher fn)
        {
            if(functions.ContainsKey(name))
            {
                throw new ArgumentException($"{name} is already a defined function");
            }

            functions.Add(name, fn);
        }

        public void AddMatch(string name, SingleValueMatcher fn)
        {
            if (functions.ContainsKey(name))
            {
                throw new ArgumentException($"{name} is already a defined function");
            }

            functions.Add(name, a => fn.Invoke(a.Single()));
        }

        public void AddMatch(string literal)
        {
            literals.Add(literal);
        }

        public bool Match(string functionName, params string[] arguments)
        {
            if (functions.TryGetValue(functionName, out var fn))
            {
                return fn.Invoke(arguments);
            }

            throw new ArgumentException($"function '{functionName}' is not registered");
        }

        public bool Match(string literalValue)
        {
            return literals.Contains(literalValue);
        }
    }
}