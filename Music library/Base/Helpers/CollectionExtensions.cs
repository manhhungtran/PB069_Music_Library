using System.Collections.Generic;

namespace Base
{
    public static class CollectionExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
