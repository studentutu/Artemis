using System;
using System.Collections.Generic;

namespace Artemis.Sample.Extensions
{
    public static class ListExtensions
    {
        public static bool TryFind<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T found)
        {
            foreach (var element in source)
            {
                if (predicate.Invoke(element))
                {
                    found = element;
                    return true;
                }
            }

            found = default;
            return false;
        }
    }
}