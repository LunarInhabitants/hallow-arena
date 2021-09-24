using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LinkedListExtensions
{
    public static int RemoveAll<T>(this LinkedList<T> list, Predicate<T> pred)
    {
        List<LinkedListNode<T>> toRemove = new List<LinkedListNode<T>>(list.Count);

        LinkedListNode<T> node = list.First;
        while (node != null)
        {
            if (pred(node.Value))
                toRemove.Add(node);

            node = node.Next;
        }


        foreach (LinkedListNode<T> n in toRemove)
        {
            list.Remove(n);
        }

        return toRemove.Count;
    }
}