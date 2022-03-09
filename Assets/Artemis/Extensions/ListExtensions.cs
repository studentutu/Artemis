using System.Collections.Generic;

namespace Artemis.Extensions
{
    internal static class ListExtensions
    {
        internal static void Remove<T>(this List<T> list, System.Predicate<T> match)
        {
            var index = list.FindIndex(match);
            if (index != -1) list.RemoveAt(index);
        }
    }
}