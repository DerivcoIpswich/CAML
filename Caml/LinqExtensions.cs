using System.Collections.Generic;

namespace System.Linq
{
    internal static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            return Enumerable
              .Range(0, 1 << (source.Count()))
              .Select(index => source
                 .Where((v, i) => (index & (1 << i)) != 0));
        }
    }
}
