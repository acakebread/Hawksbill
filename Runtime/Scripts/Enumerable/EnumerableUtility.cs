// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static partial class EnumerableUtility
{
    public static bool IsNullOrEmpty(this Array array)
    {
        return (array == null || array.Length == 0);
    }

    /// <summary>Similar to List<>.ForEach</summary>
    public static void ForAll<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
            action (item);
    }

    /// <summary>Similar to List<>.ForEach with indexing as Linq.Select</summary>
    public static void ForAll<T>(this IEnumerable<T> enumeration, Action<T, int> action)
    {
        T[] items = enumeration.ToArray ();
        for (int i = 0; i < items.Length; action (items[i], i), i++) ;
    }

    public static IEnumerable<T> TakeLessOne<T>(this IEnumerable<T> source)
    {
        using (var e = source.GetEnumerator ())
            if (e.MoveNext ())
                for (var value = e.Current; e.MoveNext (); value = e.Current)
                    yield return value;
    }

    /// <summary>Get Index of element</summary>
    public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        int index = 0;
        foreach (var item in items)
        {
            if (predicate (item)) return index;
            index++;
        }
        return -1;
    }

    /// <summary>Joins 2 enumerations together into a Tuple</summary>
    public static IEnumerable<Tuple<S, T>> Join<S, T>(this IEnumerable<S> enumeration, IEnumerable<T> with)
    {
        var items1 = enumeration.ToArray ();
        var items2 = with.ToArray ();
        for (int i = 0, n = Math.Min (items1.Length, items2.Length); i < n; i++)
            yield return new Tuple<S, T> (items1[i], items2[i]);
    }

    /// <summary>Get distinct list of objects via key selector</summary>
    public static IEnumerable<T> Unique<T, S>(this IEnumerable<T> source, Func<T, S> keySelector) =>
        source.GroupBy (o => keySelector (o)).Select (g => g.First ());

    /// <summary>Create a lookup dictionary, avoiding and reporting duplicate keys</summary>
    public static Dictionary<K, V> ToSafeDictionary<S, K, V>(this IEnumerable<S> source, Func<S, K> keySelector, Func<S, V> ValueSelector)
    {
        Dictionary<K, V> d = new Dictionary<K, V> ();
        foreach (S element in source)
        {
            var key = keySelector (element);
            if (d.ContainsKey (key)) Debug.LogWarning ("Duplicate Key found in Dictionary! Key: '" + key + "'");
            else d.Add (key, ValueSelector (element));
        }
        return d;
    }

    /// <summary>Create a lookup dictionary, avoiding and reporting duplicate keys. Gets a list of key duplications</summary>
    public static Dictionary<K, V> ToSafeDictionary<S, K, V>(this IEnumerable<S> source, Func<S, K> keySelector, Func<S, V> ValueSelector, out List<KeyValuePair<K, V>> duplicates)
    {
        Dictionary<K, V> d = new Dictionary<K, V> ();
        duplicates = new List<KeyValuePair<K, V>> ();
        foreach (S element in source)
        {
            var key = keySelector (element);
            if (d.ContainsKey (key)) duplicates.Add (new KeyValuePair<K, V> (key, ValueSelector (element)));
            else d.Add (key, ValueSelector (element));
        }
        return d;
    }

    // EQUAL ///////////////////////////////
    public static bool sequenceEqual<T, S>(this IEnumerable<T> enumeration1, IEnumerable<S> enumeration2, Func<T, S, bool> compare)
    {
        if (enumeration1.Count () == enumeration2.Count ())
        {
            return enumeration1.Join (enumeration2).All (j => compare (j.Item1, j.Item2));
            // IEnumerator<T> e1 = enumeration1.GetEnumerator ();
            // IEnumerator<S> e2 = enumeration2.GetEnumerator ();
            // while (e1.MoveNext () && e2.MoveNext ())
            //     if (!compare (e1.Current, e2.Current)) return false;
            // return true;
        }
        return false;
    }

    // RANGE ///////////////////////////////
    public static IEnumerable<float> Range(float start, float end, float step)
    {
        step = Mathf.Abs (step);
        float range = end - start, count = Mathf.Ceil (Mathf.Abs (range) / step);
        step = range / count * Mathf.Sign (range);
        for (float i = 0, v = start; i < count; i++, v += step) yield return v;
        yield return end;
    }

    public static IEnumerable<Vector2> Range(Vector2 start, Vector2 end, float count)
    {
        Vector2 step = (end - start) / count, v = start;
        for (float i = 0; i < count; i++, v += step) yield return v;
        yield return end;
    }

    public static IEnumerable<Vector3> Range(Vector3 start, Vector3 end, float count)
    {
        Vector3 step = (end - start) / count, v = start;
        for (float i = 0; i < count; i++, v += step) yield return v;
        yield return end;
    }

    public static IEnumerable<Vector4> Range(Vector4 start, Vector4 end, float count)
    {
        Vector4 step = (end - start) / count, v = start;
        for (float i = 0; i < count; i++, v += step) yield return v;
        yield return end;
    }

    public static IEnumerable<int> Range(int start, int end, int step)
    {
        for (int i = start; i < end; i += step) yield return i;
    }
}



// public static T next<T>(this IEnumerable<T> enumeration, T current)
// {
//     var list = enumeration.ToList ();
//     return list[(list.IndexOf (current) + 1) % list.Count];
// }
// public static T prev<T>(this IEnumerable<T> enumeration, T current)
// {
//     var list = enumeration.ToList ();
//     return list[(list.IndexOf (current) + list.Count - 1) % list.Count];
// }