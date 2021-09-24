using System;
using System.Collections.Generic;
using URandom = UnityEngine.Random;

public static class ArrayExtensions
{
    public static T Random<T>(this T[] array)
    {
        if (array.Length == 0)
            throw new IndexOutOfRangeException("Cannot get a random element from an empty array");
        return array[URandom.Range(0, array.Length)];
    }
}

public static class ListExtensions
{
    public static T Random<T>(this List<T> list)
    {
        if (list.Count == 0)
            throw new IndexOutOfRangeException("Cannot get a random element from an empty list");
        return list[URandom.Range(0, list.Count)];
    }
}