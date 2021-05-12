using System.Collections.Generic;

namespace Caml.Equatable
{
    internal class TermsMatcher : IMatcher
        {
            public IDictionary<string, bool> Input { get; }

            public TermsMatcher(IDictionary<string, bool> input)
            {
                Input = input;
            }

            public bool Match(string literalValue)
            {
                return Input[literalValue];
            }

            public bool Match(string functionName, params string[] arguments)
            {
                string functionString = $"{functionName}({string.Join(", ", arguments)})";
                return Input[functionString];
            }
    }
}
