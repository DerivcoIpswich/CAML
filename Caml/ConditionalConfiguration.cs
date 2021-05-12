using System.Collections.Generic;
using System.Linq;

namespace Caml
{
    public class ConditionalConfiguration<T> : List<ConditionalSection<T>>
    {
        public IEnumerable<T> GetMatches(IMatcher matcher)
        {
            return this.Where(m => m.Expression.IsMatch(matcher)).Select(i => i.Value);
        }
    }
}