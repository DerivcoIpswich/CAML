using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Caml.Test
{
    public class ConditionalSectionJsonConverterTests
    {
        const string EXAMPLE = @"{
  ""Expression"": ""hello(world) & goodbye"",
  ""Value"": {
    ""test"": ""value""
  }
}";

        const string REVERSED = @"{
  ""Value"": {
    ""test"": ""value""
  },
  ""Expression"": ""hello(world) & goodbye""
}";

        const string EXTRA = @"{
  ""Extra1"" : ""test"",
  ""Expression"": ""hello(world) & goodbye"",
  ""Extra2"" : ""test"",
  ""Value"": {
    ""test"": ""value""
  },
  ""Extra3"" : ""test""
}";

        [Fact]
        public void CanSerialise()
        {
            var section = new ConditionalSection<IDictionary<string, string>>("hello(world) & goodbye", new Dictionary<string, string> {
                { "test","value" }
            });

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(section, Newtonsoft.Json.Formatting.Indented);

            Assert.Equal(EXAMPLE, result);
        }

        [Theory]
        [InlineData(EXAMPLE)]
        [InlineData(REVERSED)]
        [InlineData(EXTRA)]
        public void CanDeserialise(string example)
        {
            var section = new ConditionalSection<IDictionary<string, string>>("hello(world) & goodbye", new Dictionary<string, string> {
                { "test","value" }
            });

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ConditionalSection<IDictionary<string, string>>>(example);

            Assert.Equal(section, result, new ConditionalSectionComparer<IDictionary<string, string>>((a, b) => a.OrderBy(kvp => kvp.Key).SequenceEqual(b.OrderBy(kvp => kvp.Key))));
        }

        private class ConditionalSectionComparer<T> : IEqualityComparer<object>
        {
            private readonly Func<T, T, bool> valueComparison;

            public ConditionalSectionComparer(Func<T, T, bool> valueComparison)
            {
                this.valueComparison = valueComparison;
            }

            public new bool Equals([AllowNull] object x, [AllowNull] object y) => Equals(x as ConditionalSection<T>, y as ConditionalSection<T>);

            public bool Equals(ConditionalSection<T> x, ConditionalSection<T> y)
            {
                if (x is null && y is null)
                {
                    return true;
                }

                return x.Expression.Equals(y?.Expression)
                    && valueComparison(x.Value, y.Value);
            }

            public int GetHashCode([DisallowNull] object obj)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
