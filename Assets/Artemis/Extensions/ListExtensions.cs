using System.Collections.Generic;

namespace Artemis.Extensions
{
    internal static class ListExtensions
    {
        internal static void Remove<T>(this List<T> list, System.Predicate<T> match)
        {
            var itemIndex = list.FindIndex(match);
            list.RemoveAt(itemIndex);
        }
    }
}