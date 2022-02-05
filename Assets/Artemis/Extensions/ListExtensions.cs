using System.Collections.Generic;

public static class ListExtensions
{
    public static void Remove<T>(this List<T> list, System.Predicate<T> match)
    {
        var itemIndex = list.FindIndex(match);
        list.RemoveAt(itemIndex);
    }
}