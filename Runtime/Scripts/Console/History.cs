// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:01 by seancooper
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class History<T>
{
    public int index { get; protected set; }
    public int count { get; protected set; }
    List<T> items = new List<T> ();

    public bool hasBack => index > 0;
    public T back() => items[index = Mathf.Max (0, index - 1)];

    public bool hasForward => index < count;
    public T forward() => items[index = Mathf.Min (items.Count, index + 1)];

    public void add(T item)
    {
        if (items.Count != count)
            items.Remove (items.Last ());
        items.Add (item);
        count = index = items.Count;
    }

    public void setCurrent(T current)
    {
        if (items.Count == count) items.Add (current);
        else items[items.Count - 1] = current;
    }

}
