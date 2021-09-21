using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DictionaryInterface<S, T> : IEnumerable<KeyValuePair<S, T>>
{
    // Data
    public Dictionary<S, T> data;
    public T defaultValue = default (T);

    // Constructors
    public DictionaryInterface(Dictionary<S, T> data, T defaultValue)
    {
        this.data = data;
        this.defaultValue = defaultValue;
    }
    public DictionaryInterface(Dictionary<S, T> data)
    {
        this.data = data;
    }
    public DictionaryInterface(IEnumerable<KeyValuePair<S, T>> data)
    {
        this.data = data.ToDictionary (p => p.Key, p => p.Value);
    }

    // public T this[S key]
    // {
    //     get
    //     {
    //         if (!data.TryGetValue (key, out T value))
    //         {
    //             Debug.LogError ("Key '" + key + "' is not found!");
    //             return default (T);
    //         }
    //         return value;
    //     }
    // }

    public T this[S key] => data.TryGetValue (key, out T value) ? value : default (T);
    public bool hasKey(S key) => data.ContainsKey (key);

    // Interface
    public Dictionary<S, T>.KeyCollection Keys => data.Keys;
    public Dictionary<S, T>.ValueCollection Values => data.Values;

    public IEnumerator<KeyValuePair<S, T>> GetEnumerator() { foreach (var p in data) yield return p; }
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator ();
    public static implicit operator bool(DictionaryInterface<S, T> empty) => empty != null && empty.data != null;
}